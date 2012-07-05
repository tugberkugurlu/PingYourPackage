function require-param { 
    param($value, $paramName)
    
    if($value -eq $null) { 
        write-error "The parameter -$paramName is required."
    }
}

function print-error { 
    param($value)
    
    Write-Host $value -ForegroundColor White -BackgroundColor Red
}

function print-warning { 
    param($value)
    
    Write-Host $value -ForegroundColor Red -BackgroundColor Yellow   
}

function print-success {
    param($value)
    Write-Host $value -foregroundcolor Black -backgroundcolor Green
}

function require-module { 
    param([string]$name)
    if(-not(Get-Module -Name $name)) { 
        if(Get-Module -ListAvailable | Where-Object {$_.Name -eq $name }) { 
            Write-Host "Module is avalible and will be loaded."
            Import-Module -Name $name
        } else { 
            write-error "The module '$name' is required."
            exit 1
        }
    }
}

function programfiles-dir {
    if (is64bit -eq $true) {
        (Get-Item "Env:ProgramFiles(x86)").Value
    } else {
        (Get-Item "Env:ProgramFiles").Value
    }
}

function is64bit() {
    return ([IntPtr]::Size -eq 8)
}