# Define the initial component name prefix
$componentPrefix = "My Components.*"

# Create the COMAdminCatalog object
$catalog = New-Object -ComObject COMAdmin.COMAdminCatalog

# Get the COM+ applications
$applications = $catalog.GetCollection("Applications")
$applications.Populate()

# Iterate over each application to get the components
$results = @()

foreach ($app in $applications) {
    $components = $catalog.GetCollection("Components", $app.Key)
    $components.Populate()
    
    foreach ($component in $components) {
        # Filter components that support transactions and whose name starts with the specified prefix
        if ($component.Name -like $componentPrefix -and $component.Transaction > 0) {
            $results += [PSCustomObject]@{
                ApplicationName = $app.Name
                ComponentName = $component.Name
                TransactionSupport = $component.Transaction
            }
        }
    }
}

# Display the results
$results
