# Content of common-setup.ps1
function ConfigureFirewallForComPlusDistributed {
    $ruleName = "COM+ DCOM"

    # Check if rules already exist, and if so, remove them
    $existingInboundRule = Get-NetFirewallRule | Where-Object {($_.Name -eq "$ruleName Inbound")}
    $existingOutboundRule = Get-NetFirewallRule | Where-Object {($_.Name -eq "$ruleName Outbound")}
    if ($existingInboundRule -ne $null) {
        Write-Host "Removing existing rule (inbound)..."
        Remove-NetFirewallRule -Name $existingInboundRule.Name
    }
    if ($existingOutboundRule -ne $null) {
        Write-Host "Removing existing rule (outbound)..."
        Remove-NetFirewallRule -Name $existingOutboundRule.Name
    }

    # Create new firewall rules to allow COM+ distributed traffic (inbound and outbound)
    Write-Host "Creating new firewall rules (inbound and outbound)..."
    New-NetFirewallRule -Name "$ruleName Inbound" -DisplayName "Allow COM+ DCOM Inbound" -Enabled True -Direction Inbound -Action Allow -Protocol TCP -LocalPort 135
    New-NetFirewallRule -Name "$ruleName Outbound" -DisplayName "Allow COM+ DCOM Outbound" -Enabled True -Direction Outbound -Action Allow -Protocol TCP -LocalPort 135

    # Configure the firewall for COM+ distributed (inbound and outbound)
    Write-Host "Configuring the firewall for COM+ distributed (inbound and outbound)..."
    Set-NetFirewallRule -Name "$ruleName Inbound" -RemoteAddress Any
    Set-NetFirewallRule -Name "$ruleName Outbound" -RemoteAddress Any
}


function EnableComPlusRemoteAccess {
    $regKeyPath = "HKLM:\SOFTWARE\Microsoft\COM3"
    $regValueName = "RemoteAccessEnabled"
    $regValueData = 1

    # Verifica si la clave del Registro existe, y si no, créala
    if (-not (Test-Path $regKeyPath)) {
        Write-Host "Creating Registry key..."
        New-Item -Path $regKeyPath -Force
    }

    # Configura el valor del Registro para habilitar el acceso remoto COM+
    Write-Host "Setting Registry value to enable COM+ remote access..."
    Set-ItemProperty -Path $regKeyPath -Name $regValueName -Value $regValueData
}

# Configure Client and Administration
function Set-DTCClientAndAdmin {
    param (
        [bool] $adminRemoteAccessEnabled,
        [bool] $allowAdmin
    )

    $dtc = Get-Dtc

    $dtc.Security.ClientAndAdminAllowRemote = $adminRemoteAccessEnabled
    $dtc.Security.AllowAdmin = $allowAdmin

    $dtc | Set-Dtc

    Write-Host "Client and Administration settings configured successfully."
}

# Configure Transaction Manager Communication
function Set-DTCTransactionManagerCommunication {
    param (
        [bool] $networkClientAccessEnabled,
        [bool] $allowInbound
    )

    $dtc = Get-Dtc

    $dtc.Security.NetworkClientAccessEnabled = $networkClientAccessEnabled
    $dtc.Security.NetworkInboundAccess = $allowInbound

    $dtc | Set-Dtc

    Write-Host "Transaction Manager Communication settings configured successfully."
}

# Configure XA and SNA settings
function Set-DTCXAandSNASettings {
    param (
        [bool] $enableXADistributedTransaction,
        [bool] $enableSNADistributedTransaction
    )

    $dtc = Get-Dtc

    $dtc.Security.XaTransactionsEnabled = $enableXADistributedTransaction
    $dtc.Security.SnaLuTransactionsEnabled = $enableSNADistributedTransaction

    $dtc | Set-Dtc

    Write-Host "XA and SNA settings configured successfully."
}
