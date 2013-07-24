$cert = Get-ChildItem Cert:\LocalMachine\My | where { $_.Subject -eq 'CN=localhost' }

if($cert -ne $null) { 

    Write-Host 'INFO: IIS Express certificate has been found' -ForegroundColor Cyan

    $store = (Get-Item Cert:\LocalMachine\Root)
    $store.Open("ReadWrite")
    $store.Add($cert)
    $store.Close()
    
    Write-Host 'SUCCESS: IIS Express certificate has been added into the Trusted Root Certificates store' -ForegroundColor Green
}
else {

    Write-Host 'WARN: Could not find the proper certificate!' -ForegroundColor Red
}