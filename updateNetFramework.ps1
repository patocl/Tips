# Path to your Visual Studio solution
$solutionPath = "Your\Solution\Path"

# Version of .NET Framework you want to update to
$newFrameworkVersion = "4.8"

# Iterate through all .csproj and .vbproj files in the solution and update the framework version
Get-ChildItem -Path $solutionPath -Filter *.csproj, *.vbproj -Recurse | ForEach-Object {
    $projectFile = $_.FullName
    Write-Host "Processing $projectFile..."

    # Read the content of the project file
    $projectContent = Get-Content $projectFile

    # Replace the framework version in the project file
    $updatedProjectContent = $projectContent -replace '<TargetFrameworkVersion>v\d+\.\d+<\/TargetFrameworkVersion>', "<TargetFrameworkVersion>v$newFrameworkVersion<\/TargetFrameworkVersion>"

    # Write the updated content back to the project file
    $updatedProjectContent | Set-Content $projectFile

    Write-Host "Updated to .NET Framework $newFrameworkVersion"
}