. ".\common-setup.ps1"

function Set-IISAuthentication {
    param (
        [string] $websiteName
    )

    $iisPath = "IIS:\Sites\$websiteName"

    # Disable Basic and Anonymous authentication
    Set-WebConfigurationProperty -PSPath $iisPath -Filter "system.webServer/security/authentication/windowsAuthentication" -Name "enabled" -Value $false
    Set-WebConfigurationProperty -PSPath $iisPath -Filter "system.webServer/security/authentication/anonymousAuthentication" -Name "enabled" -Value $false

    # Enable Negotiate:Kerberos authentication and set it to use kernel mode
    Set-WebConfigurationProperty -PSPath $iisPath -Filter "system.webServer/security/authentication/windowsAuthentication" -Name "enabled" -Value $true
    Set-WebConfigurationProperty -PSPath $iisPath -Filter "system.webServer/security/authentication/windowsAuthentication" -Name "useKernelMode" -Value $true
}

function Set-IISApplicationPoolGeneralProperties {
    param (
        [string] $applicationPoolName,
        [int] $managedRuntimeVersion,
        [bool] $enable32BitAppOnWin64,
        [bool] $managedPipelineMode,
        [string] $clrConfigFile,
        [string] $startMode
    )

    # Get the Application Pool object
    $appPool = Get-WebAppPool -Name $applicationPoolName

    # Configure General properties
    $appPool.managedRuntimeVersion = "v$managedRuntimeVersion"
    $appPool.enable32BitAppOnWin64 = $enable32BitAppOnWin64
    $appPool.managedPipelineMode = $managedPipelineMode
    $appPool.managedPipelineMode = $clrConfigFile
    $appPool.startMode = $startMode

    # Apply the configuration
    Set-WebAppPool -InputObject $appPool

    Write-Host "General properties configured for Application Pool: $applicationPoolName"
}

function Set-IISApplicationPoolCPU {
    param (
        [string] $applicationPoolName,
        [int] $cpuLimit,
        [int] $actionType
    )

    # Get the Application Pool object
    $appPool = Get-WebAppPool -Name $applicationPoolName

    # Configure CPU limits
    $appPool.cpu.limit = $cpuLimit

<#
    Possible values for $actionType are as follows:
    0 for No Action.
    1 for KillW3wp.
    2 for Throttle.
#> 
    $appPool.cpu.action = $actionType

    # Apply the configuration
    Set-WebAppPool -InputObject $appPool

    Write-Host "CPU limit configured for Application Pool: $applicationPoolName"
}

function Set-IISApplicationPoolProcessModelProperties {
    param (
        [string] $applicationPoolName,
        [int] $maxProcesses,
        [int] $shutdownTimeLimit,
        [int] $idleTimeout,
        [bool] $pingingEnabled,
        [int] $pingInterval,
        [bool] $pingResponseTime,
        [int] $identityType
    )

    # Get the Application Pool object
    $appPool = Get-WebAppPool -Name $applicationPoolName

    # Configure the Process Model properties
    $appPool.processModel.maxProcesses = $maxProcesses
    $appPool.processModel.shutdownTimeLimit = [TimeSpan]::FromMinutes($shutdownTimeLimit)
    $appPool.processModel.idleTimeout = [TimeSpan]::FromMinutes($idleTimeout)
    $appPool.processModel.pingingEnabled = $pingingEnabled
    $appPool.processModel.pingInterval = [TimeSpan]::FromMinutes($pingInterval)
    $appPool.processModel.pingResponseTime = [TimeSpan]::FromMinutes($pingResponseTime)
    $appPool.processModel.identityType = $identityType

    # Apply the configuration
    Set-WebAppPool -InputObject $appPool

    Write-Host "Process Model properties configured for Application Pool: $applicationPoolName"
}

function Set-IISApplicationPoolProcessOrphaning {
    param (
        [string] $applicationPoolName,
        [bool] $orphanActionExeEnabled,
        [string] $orphanActionExePath,
        [string] $orphanActionExeParams,
        [bool] $orphanWorkerProcessEnabled
    )

    # Get the Application Pool object
    $appPool = Get-WebAppPool -Name $applicationPoolName

    # Configure Process Orphaning settings
    $appPool.processModel.orphanActionExeEnabled = $orphanActionExeEnabled
    $appPool.processModel.orphanActionExe = $orphanActionExePath
    $appPool.processModel.orphanActionParams = $orphanActionExeParams
    $appPool.processModel.orphanWorkerProcess = $orphanWorkerProcessEnabled

    # Apply the configuration
    Set-WebAppPool -InputObject $appPool

    Write-Host "Process Orphaning configured for Application Pool: $applicationPoolName"
}

function Set-IISApplicationPoolRapidFailProtection {
    param (
        [string] $applicationPoolName,
        [bool] $enabled,
        [int] $failureInterval,
        [int] $failureCount,
        [int] $autoResetInterval
    )

    # Get the Application Pool object
    $appPool = Get-WebAppPool -Name $applicationPoolName

    # Configure all Rapid-Fail Protection properties
    $appPool.failure.rapidFailProtection = $enabled
    $appPool.failure.rapidFailProtectionInterval = [TimeSpan]::FromMinutes($failureInterval)
    $appPool.failure.rapidFailProtectionMaxCrashes = $failureCount
    $appPool.failure.autoShutDown = $true
    $appPool.failure.autoShutDownInterval = [TimeSpan]::FromMinutes($autoResetInterval)

    # Apply the configuration
    Set-WebAppPool -InputObject $appPool

    Write-Host "Rapid Fail Protection configured for Application Pool: $applicationPoolName"
}

function Set-IISApplicationPoolRecycling {
    param (
        [string] $applicationPoolName,
        [int] $privateMemoryLimit,
        [int] $regularTimeInterval,
        [int] $specificTime,
        [int] $specificTimePeriod,
        [int] $maxProcesses
    )

    # Get the Application Pool object
    $appPool = Get-WebAppPool -Name $applicationPoolName

    # Configure Recycling properties
    $appPool.recycling.logEventOnRecycle = "Time,Requests,Schedule,Memory"
    $appPool.recycling.periodicRestart.schedule.clear()
    $appPool.recycling.periodicRestart.schedule.add("00:00:00")
    $appPool.recycling.periodicRestart.time = [TimeSpan]::FromMinutes($regularTimeInterval)
    $appPool.recycling.periodicRestart.privateMemory = $privateMemoryLimit
    $appPool.recycling.periodicRestart.memory = $true
    $appPool.recycling.periodicRestart.requests = $false
    $appPool.recycling.periodicRestart.scheduleCollection.clear()
    $appPool.recycling.periodicRestart.scheduleCollection.add("00:00:00")
    $appPool.recycling.periodicRestart.scheduleCollection.add("00:00:00")
    $appPool.failure.rapidFailProtection = $true
    $appPool.failure.rapidFailProtectionInterval = [TimeSpan]::FromMinutes(5)
    $appPool.failure.maxProcesses = $maxProcesses

    # Apply the configuration
    Set-WebAppPool -InputObject $appPool

    Write-Host "Recycling properties configured for Application Pool: $applicationPoolName"
}

# Common Setup
ConfigureFirewallForComPlusDistributed
EnableComPlusRemoteAccess

# IIS Setup
Set-IISAuthentication -websiteName "VCA"

# Invoke the functions with the specified Application Pool name
$applicationPoolName = "VCA"

Set-IISApplicationPoolGeneralProperties -applicationPoolName $applicationPoolName -managedRuntimeVersion 4.0 -enable32BitAppOnWin64 $true -managedPipelineMode Integrated -clrConfigFile "C:\Path\To\Your\ConfigFile" -startMode AlwaysRunning
Set-IISApplicationPoolCPU -applicationPoolName $applicationPoolName -cpuLimit 50 -actionType 1
Set-IISApplicationPoolProcessModelProperties -applicationPoolName $applicationPoolName -maxProcesses 4 -shutdownTimeLimit 15 -idleTimeout 20 -pingingEnabled $true -pingInterval 5 -pingResponseTime $true -identityType 3
Set-IISApplicationPoolProcessOrphaning -applicationPoolName $applicationPoolName -orphanActionExeEnabled $true -orphanActionExePath "C:\Path\To\YourExe.exe" -orphanActionExeParams "-param1 value1 -param2 value2" -orphanWorkerProcessEnabled $true
Set-IISApplicationPoolRapidFailProtection -applicationPoolName $applicationPoolName -enabled $true -failureInterval 5 -failureCount 5 -autoResetInterval 60
Set-IISApplicationPoolRecycling -applicationPoolName $applicationPoolName -privateMemoryLimit 1024000 -regularTimeInterval 1740 -specificTime 0 -specificTimePeriod 0 -maxProcesses 5

# Services Components Setup
Set-DTCClientAndAdmin -adminRemoteAccessEnabled $true -allowAdmin $true
Set-DTCTransactionManagerCommunication -networkClientAccessEnabled $true -allowInbound $true
Set-DTCXAandSNASettings -enableXADistributedTransaction $true -enableSNADistributedTransaction $true
