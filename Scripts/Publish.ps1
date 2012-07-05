param(
    $subscriptionId         	= $env:PINGYOURPACKAGE_AZURE_SUBSCRIPTION_ID,
    $serviceName            	= $env:PINGYOURPACKAGE_WEB_AZURE_SERVICE_NAME,
    $storageServiceName     	= $env:PINGYOURPACKAGE_WEB_AZURE_STORAGE_ACCOUNT_NAME,
	$sqlAzureConnectionString   = $env:PINGYOURPACKAGE_SQL_AZURE_CONNECTION_STRING
)

# Import Common Stuff
$ScriptRoot = (Split-Path -parent $MyInvocation.MyCommand.Definition)
. $ScriptRoot\_Common.ps1

# Validate Sutff
require-param -value $subscriptionId -paramName "subscriptionId"
require-param -value $serviceName -paramName "serviceName"
require-param -value $storageServiceName -paramName "storageServiceName"
require-param -value $sqlAzureConnectionString -paramName "sqlAzureConnectionString"

# Helper Functions
function set-configurationsetting {
  param($path, $name, $value)
  $xml = [xml](get-content $path)
  $setting = $xml.serviceconfiguration.role.configurationsettings.setting | where { $_.name -eq $name }
  $setting.value = "$value"
  $resolvedPath = resolve-path($path) 
  $xml.save($resolvedPath)
}

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