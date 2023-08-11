using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Retry;

public abstract class RetryHealthCheckBase : IHealthCheck
{
    private readonly RetryPolicy _retryPolicy;

    protected RetryHealthCheckBase()
    {
        // Configure the retry policy (3 retries with exponential backoff)
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    protected abstract Task<HealthCheckResult> PerformCheckAsync(HealthCheckContext context);

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context)
    {
        // Execute the PerformCheckAsync method within the retry policy
        return _retryPolicy.ExecuteAsync(() => PerformCheckAsync(context));
    }
}

public class SharedFolderHealthCheck : RetryHealthCheckBase
{
    private readonly SharedFolderInfo _sharedFolderInfo;

    public SharedFolderHealthCheck(SharedFolderInfo sharedFolderInfo)
    {
        _sharedFolderInfo = sharedFolderInfo ?? throw new ArgumentNullException(nameof(sharedFolderInfo));
    }

    protected override async Task<HealthCheckResult> PerformCheckAsync(HealthCheckContext context)
    {
        try
        {
            var directoryInfo = new DirectoryInfo(_sharedFolderInfo.FolderPath);

            if (!directoryInfo.Exists)
            {
                return HealthCheckResult.Unhealthy($"Folder '{_sharedFolderInfo.FolderPath}' does not exist.");
            }

            var userPrincipal = new WindowsPrincipal(new WindowsIdentity(_sharedFolderInfo.UserName));
            var accessRules = directoryInfo.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));

            var fileAccess = _sharedFolderInfo.FileAccess;
            var folderAccess = _sharedFolderInfo.FolderAccess;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if (userPrincipal.IsInRole(rule.IdentityReference as SecurityIdentifier))
                {
                    if ((fileAccess.CanRead && (rule.FileSystemRights & FileSystemRights.Read) == 0) ||
                        (fileAccess.CanWrite && (rule.FileSystemRights & FileSystemRights.Write) == 0) ||
                        (fileAccess.CanDelete && (rule.FileSystemRights & FileSystemRights.Delete) == 0) ||
                        (fileAccess.CanRename && (rule.FileSystemRights & FileSystemRights.Modify) == 0) ||
                        (fileAccess.CanCreate && (rule.FileSystemRights & FileSystemRights.CreateFiles) == 0))
                    {
                        return HealthCheckResult.Unhealthy($"User '{_sharedFolderInfo.UserName}' does not have required file permissions on folder '{_sharedFolderInfo.FolderPath}'.");
                    }

                    if (folderAccess.CanList && !_sharedFolderInfo.TestListFolderAccess(directoryInfo))
                    {
                        return HealthCheckResult.Unhealthy($"User '{_sharedFolderInfo.UserName}' does not have required list folder permissions on folder '{_sharedFolderInfo.FolderPath}'.");
                    }
                }
            }

            return HealthCheckResult.Healthy($"Folder '{_sharedFolderInfo.FolderPath}' is reachable and user '{_sharedFolderInfo.UserName}' has required permissions.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Folder check failed for '{_sharedFolderInfo.FolderPath}'.", ex);
        }
    }
}

public class SharedFolderInfo
{
    public string FolderPath { get; set; }
    public string UserName { get; set; }
    public FileAccessInfo FileAccess { get; set; }
    public FolderAccessInfo FolderAccess { get; set; }

    public bool TestListFolderAccess(DirectoryInfo directoryInfo)
    {
        try
        {
            _ = directoryInfo.GetFiles();
            _ = directoryInfo.GetDirectories();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }
}

public class FileAccessInfo
{
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool CanDelete { get; set; }
    public bool CanRename { get; set; }
    public bool CanCreate { get; set; }
}

public class FolderAccessInfo
{
    public bool CanList { get; set; }
    public bool CanCreate { get; set; }
}
