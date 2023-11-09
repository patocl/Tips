. ".\common-setup.ps1"

# Common Setup
ConfigureFirewallForComPlusDistributed
EnableComPlusRemoteAccess

# Services Components Setup
Set-DTCClientAndAdmin -adminRemoteAccessEnabled $true -allowAdmin $true
Set-DTCTransactionManagerCommunication -networkClientAccessEnabled $true -allowInbound $true
Set-DTCXAandSNASettings -enableXADistributedTransaction $true -enableSNADistributedTransaction $true
