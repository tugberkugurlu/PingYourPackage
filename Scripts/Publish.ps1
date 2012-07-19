param(
    $subscriptionId                     = $env:PINGYOURPACKAGE_AZURE_SUBSCRIPTION_ID,
    $certificateThumbprint              = $env:PINGYOURPACKAGE_MANAGEMENT_CERTIFICATE_THUMBPRINT,
    $serviceName                        = $env:PINGYOURPACKAGE_AZURE_SERVICE_NAME,
    $storageServiceName                 = $env:PINGYOURPACKAGE_AZURE_STORAGE_ACCOUNT_NAME,
    $sqlAzureConnectionString           = $env:PINGYOURPACKAGE_SQL_AZURE_CONNECTION_STRING,
    $sslCertificateThumbprint           = $env:PINGYOURPACKAGE_SSL_CERTIFICATE_THUMBPRINT,
    $remoteDesktopAccountExpiration     = $env:PINGYOURPACKAGE_REMOTE_DESKTOP_ACCOUNT_EXPIRATION,
    $remoteDesktopCertificateThumbprint = $env:PINGYOURPACKAGE_REMOTE_DESKTOP_CERTIFICATE_THUMBPRINT,
    $remoteDesktopPassword              = $env:PINGYOURPACKAGE_REMOTE_DESKTOP_PASSWORD,
    $remoteDesktopUsername              = $env:PINGYOURPACKAGE_REMOTE_DESKTOP_USERNAME,
    $commitSha,
    $commitBranch
)

# Import Common Stuff
$ScriptRoot = (Split-Path -parent $MyInvocation.MyCommand.Definition)
. $ScriptRoot\_Common.ps1

# Validate Sutff
require-param -value $subscriptionId -paramName "subscriptionId"
require-param -value $certificateThumbprint -paramName "certificateThumbprint"
require-param -value $serviceName -paramName "apiServiceName"
require-param -value $storageServiceName -paramName "storageServiceName"
require-param -value $sqlAzureConnectionString -paramName "sqlAzureConnectionString"
require-param -value $remoteDesktopAccountExpiration -paramName "remoteDesktopAccountExpiration"
require-param -value $remoteDesktopCertificateThumbprint -paramName "remoteDesktopCertificateThumbprint"
require-param -value $remoteDesktopPassword -paramName "remoteDesktopPassword"
require-param -value $remoteDesktopUsername -paramName "remoteDesktopUsername"

require-module -name "WAPPSCmdlets"

# Helper Functions
function set-connectionstring {
  param($path, $name, $value)
  $settings = [xml](get-content $path)
  $setting = $settings.configuration.connectionStrings.add | where { $_.name -eq $name }
  $setting.connectionString = "$value"
  $setting.providerName = "System.Data.SqlClient"
  $resolvedPath = resolve-path($path) 
  $settings.save($resolvedPath)
}

function set-appsetting {
    param($path, $name, $value)
    $settings = [xml](get-content $path)
    $setting = $settings.configuration.appSettings.selectsinglenode("add[@key='" + $name + "']")
    $setting.value = $value.toString()
    $resolvedPath = resolve-path($path) 
    $settings.save($resolvedPath)
}

function set-releasemode {
  param($path)
  $xml = [xml](get-content $path)
  $compilation = $xml.configuration."system.web".compilation
  $compilation.debug = "false"
  $resolvedPath = resolve-path($path) 
  $xml.save($resolvedPath)
}

function encrypt-envelope ($unprotectedContent, $cert) {
	[System.Reflection.Assembly]::LoadWithPartialName("System.Security") | Out-Null
    $utf8content = [Text.Encoding]::UTF8.GetBytes($unprotectedContent)
    $content = New-Object Security.Cryptography.Pkcs.ContentInfo -argumentList (,$utf8content)
	$env = New-Object Security.Cryptography.Pkcs.EnvelopedCms $content
	$recpient = (New-Object System.Security.Cryptography.Pkcs.CmsRecipient($cert))
	$env.Encrypt($recpient)
	$base64string = [Convert]::ToBase64String($env.Encode())
	return $base64string
}

function set-configurationsetting {
  param($path, $name, $value)
  $xml = [xml](get-content $path)
  $setting = $xml.serviceconfiguration.role.configurationsettings.setting | where { $_.name -eq $name }
  $setting.value = "$value"
  $resolvedPath = resolve-path($path) 
  $xml.save($resolvedPath)
}

function set-certificatethumbprint {
  param($path, $name, $value)
  $xml = [xml](get-content $path)
  $certificate = $xml.serviceconfiguration.role.Certificates.Certificate | where { $_.name -eq $name }
  $certificate.thumbprint = "$value"
  $resolvedPath = resolve-path($path) 
  $xml.save($resolvedPath)
}

function await-operation($operationId) {

	$status = Get-OperationStatus -SubscriptionId $subscriptionId -Certificate $certificate -OperationId $operationId
	while ([string]::Equals($status, "InProgress")) {
	
		sleep -Seconds 1
		$status = Get-OperationStatus -SubscriptionId $subscriptionId -Certificate $certificate -OperationId $operationId
	}
	return $status
}

function await-status($status) {

	$deployment = Get-Deployment -ServiceName $serviceName -Slot Staging -Certificate $certificate -SubscriptionId $subscriptionId
	while (-not([string]::Equals($deployment.status, $status))) {
	
		sleep -Seconds 1
		$deployment = Get-Deployment -ServiceName $serviceName -Slot Staging -Certificate $certificate -SubscriptionId $subscriptionId
	}
}

function await-start() {

	$deployment = Get-Deployment -ServiceName $serviceName -Slot Staging -Certificate $certificate -SubscriptionId $subscriptionId
	$roleInstances = Get-RoleInstanceStatus -ServiceName $serviceName -RoleInstanceList $deployment.RoleInstanceList -Certificate $certificate -SubscriptionId $subscriptionId
	$roleInstancesThatAreNotReady = $roleInstances.RoleInstances | where-object { $_.InstanceStatus -ne "Ready" }
	while ($roleInstancesThatAreNotReady.Count -gt 0) {
	
		sleep -Seconds 1
		$deployment = Get-Deployment -ServiceName $serviceName -Slot Staging -Certificate $certificate -SubscriptionId $subscriptionId
		$roleInstances = Get-RoleInstanceStatus -ServiceName $serviceName -RoleInstanceList $deployment.RoleInstanceList -Certificate $certificate -SubscriptionId $subscriptionId
		$roleInstancesThatAreNotReady = $roleInstances.RoleInstances | where-object { $_.InstanceStatus -ne "Ready" }
	}

	$deployment = Get-Deployment -ServiceName $serviceName -Slot Staging -Certificate $certificate -SubscriptionId $subscriptionId
	while (-not([string]::Equals($deployment.status, "Running"))) {
	
		sleep -Seconds 1
		$deployment = Get-Deployment -ServiceName $serviceName -Slot Staging -Certificate $certificate -SubscriptionId $subscriptionId
	}
}

$scriptPath = split-path $MyInvocation.MyCommand.Path
$rootPath = resolve-path(join-path $scriptPath "..")
$csdefFile = join-path $scriptPath "PingYourPackage.csdef"
$artifactsPath = join-path $rootPath "artifacts"
$webAppsPath = join-path $artifactsPath "_PublishedWebsites"
$apiAppPath = join-path $webAppsPath "PingYourPackage.API.WebHost"
$webClientAppPath = join-path $webAppsPath "PingYourPackage.WebClient"
$apiAppWebConfigPath = join-path $apiAppPath "Web.config"
$apiAppWebConfigBakPath = join-path $apiAppPath "Web.config.bak"
$webClientAppWebConfigPath = join-path $webClientAppPath "Web.config"
$webClientAppWebConfigBakPath = join-path $webClientAppPath "Web.config.bak"
$rolePropertiesPath = join-path $scriptPath "PingYourPackage.RoleProperties.txt"
$cscfgPath = join-path $scriptPath "PingYourPackage.cscfg"
$cscfgBakPath = join-path $scriptPath "PingYourPackage.cscfg.bak"
$cspkgFolder = join-path $rootPath "_AzurePackage"
$cspkgFile = join-path $cspkgFolder "PingYourPackage.cspkg"
$apiAppBinPath = join-path $apiAppPath "bin"
$webClientAppBinPath = join-path $webClientAppPath "bin"
$gitPath = join-path (programfiles-dir) "Git\bin\git.exe"

"Building deployment name from date"
$deploymentLabel = "$((get-date).ToString("MMM dd @ HHmm"))"
$deploymentName = "$((get-date).ToString("yyyyMMddHHmmss"))-auto"
"Deployment name has been built: $deploymentName"

if ($commitSha -eq $null) {
	"Retrieving the commiy SHA"
    $commitSha = (& "$gitPath" rev-parse HEAD)
	"Commit SHA has been retrieved: $commitSha"
}

if ($commitBranch -eq $null) {
	"Retrieving the commit branch"
    $commitBranch = (& "$gitPath" name-rev --name-only HEAD)
	"Commit branch has been retrieved: $commitBranch"
}

cp $apiAppWebConfigPath $apiAppWebConfigBakPath
cp $webClientAppWebConfigPath $webClientAppWebConfigBakPath
cp $cscfgPath $cscfgBakPath

set-connectionstring -path $apiAppWebConfigPath -name "PingYourPackage" -value $sqlAzureConnectionString
set-appsetting $apiAppWebConfigPath "PingYourPackage:CommitSha" $commitSha
set-appsetting $webClientAppWebConfigPath "PingYourPackage:CommitSha" $commitSha
set-appsetting $apiAppWebConfigPath "PingYourPackage:CommitBranch" $commitBranch
set-appsetting $webClientAppWebConfigPath "PingYourPackage:CommitBranch" $commitBranch
set-appsetting $apiAppWebConfigPath "PingYourPackage:DeploymentName" $deploymentName
set-appsetting $webClientAppWebConfigPath "PingYourPackage:DeploymentName" $deploymentName
set-releasemode $apiAppWebConfigPath
set-releasemode $webClientAppWebConfigPath
set-certificatethumbprint -path $cscfgPath -name "Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" -value $remoteDesktopCertificateThumbprint
set-configurationsetting -path $cscfgPath -name "Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" -value $remoteDesktopAccountExpiration
set-configurationsetting -path $cscfgPath -name "Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" -value (encrypt-envelope $remoteDesktopPassword (get-item cert:\CurrentUser\My\$remoteDesktopCertificateThumbprint))
set-configurationsetting -path $cscfgPath -name "Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" -value $remoteDesktopUsername

if ((test-path $cspkgFolder) -eq $false) {
  mkdir $cspkgFolder | out-null
} else {
  remove-item $cspkgFolder\* -Recurse
}

& 'C:\Program Files\Windows Azure SDK\v1.6\bin\cspack.exe' "$csdefFile" /out:"$cspkgFile" /role:"PingYourPackage;$webAppsPath" /sites:"PingYourPackage;PingYourPackage.API;$apiAppPath;PingYourPackage.WebClient;$webClientAppPath" /rolePropertiesFile:"PingYourPackage;$rolePropertiesPath"

cp $cscfgPath $cspkgFolder

cp $apiAppWebConfigBakPath $apiAppWebConfigPath
cp $webClientAppWebConfigBakPath $webClientAppWebConfigPath
cp $cscfgBakPath $cscfgPath

print-success("Azure package and configuration dropped to $cspkgFolder.")
write-host ""

"Getting Azure management certificate $certificateThumbprint"
$certificate = (Get-Item cert:\CurrentUser\My\$certificateThumbprint)

"Locating Azure package and configuration"
$readyCspkgFile = join-path $cspkgFolder "PingYourPackage.cspkg"
$readyCscfgFile = join-path $cspkgFolder "PingYourPackage.cscfg"

"Checking for existing staging deployment on $serviceName"
$deployment = Get-Deployment -ServiceName $serviceName -Slot Staging -Certificate $certificate -SubscriptionId $subscriptionId
if ($deployment -ne $null -and $deployment.Name -ne $null) { 
    "Deleting existing staging deployment $($deployment.Name) on $serviceName"
    $operationId = (remove-deployment -subscriptionId $subscriptionId -certificate $certificate -slot Staging -serviceName $serviceName ).operationId
    await-operation($operationId)
}

"Creating new staging deployment $deploymentName on $serviceName"
$operationId = (new-deployment -subscriptionId $subscriptionId -certificate $certificate -ServiceName $serviceName -storageServiceName $storageServiceName -slot Staging -Package $readyCspkgFile -Configuration $readyCscfgFile -Name $deploymentName -Label $deploymentLabel).operationId
await-operation($operationId)
await-status("Suspended")

"Starting staging deployment $deploymentName on $serviceName"
$operationId = (set-deploymentstatus -subscriptionId $subscriptionId -serviceName $serviceName -slot Staging -status Running -certificate $certificate).operationId
await-operation($operationId)
await-start

"Moving staging deployment $deploymentName to productionon on $serviceName"
$operationId = (move-deployment -subscriptionId $subscriptionId -serviceName $serviceName -certificate $certificate -name $deploymentName).operationId
await-operation($operationId)

print-success("Deployment $deploymentName on $serviceName has been completed")

"Deleting staging deployment $deploymentName on $serviceName"
$operationId = (remove-deployment -subscriptionId $subscriptionId -certificate $certificate -slot Staging -serviceName $serviceName ).operationId
await-operation($operationId)

Exit 0