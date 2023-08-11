using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

public interface IHealthCheck
{
    bool IsHealthy(out string reason);
}

public class HealthCheckBase : IHealthCheck
{
    protected bool CheckResult;
    protected string Reason;

    public bool IsHealthy(out string reason)
    {
        reason = Reason;
        return CheckResult;
    }
}

public class DatabaseHealthCheck : HealthCheckBase
{
    private readonly string _databaseConnectionString;

    public DatabaseHealthCheck(string databaseConnectionString)
    {
        _databaseConnectionString = databaseConnectionString;
        // Perform database check with retries
        CheckResult = PerformDatabaseCheckWithRetries(out Reason);
    }

    private bool PerformDatabaseCheckWithRetries(out string reason)
    {
        int retryCount = 3;

        var policy = Policy
            .Handle<Exception>() // Handle exceptions
            .Retry(retryCount, (ex, retry) =>
            {
                // Optional logic when retrying
            });

        try
        {
            policy.Execute(() =>
            {
                // Logic to check the health of the database using _databaseConnectionString
                // If an exception occurs, Polly will perform retries
                // Return true if healthy, false if not
            });

            reason = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            reason = $"Error while checking database: {ex.Message}";
            return false;
        }
    }
}

public class SharedFolderHealthCheck : HealthCheckBase
{
    private readonly SharedFolderConfig _sharedFolderConfig;

    public SharedFolderHealthCheck(SharedFolderConfig sharedFolderConfig)
    {
        _sharedFolderConfig = sharedFolderConfig;
        // Perform shared folder check with retries
        CheckResult = PerformSharedFolderCheckWithRetries(out Reason);
    }

    private bool PerformSharedFolderCheckWithRetries(out string reason)
    {
        int retryCount = 3;

        var policy = Policy
            .Handle<Exception>() // Handle exceptions
            .Retry(retryCount, (ex, retry) =>
            {
                // Optional logic when retrying
            });

        try
        {
            policy.Execute(() =>
            {
                // Logic to check the health of the shared folder using _sharedFolderConfig
                // If an exception occurs, Polly will perform retries
                // Return true if healthy, false if not
            });

            reason = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            reason = $"Error while checking shared folder: {ex.Message}";
            return false;
        }
    }
}

public class SharedFolderConfig
{
    public string Path { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class HealthCheckManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly bool _verbose;

    public HealthCheckManager(IServiceProvider serviceProvider, bool verbose)
    {
        _serviceProvider = serviceProvider;
        _verbose = verbose;
    }

    public void RunHealthChecks()
    {
        IEnumerable<IHealthCheck> healthChecks = _serviceProvider.GetServices<IHealthCheck>();

        foreach (var healthCheck in healthChecks)
        {
            bool isHealthy = healthCheck.IsHealthy(out string reason);
            Console.WriteLine($"[{healthCheck.GetType().Name}] Health Status: {isHealthy}");

            if (!isHealthy && _verbose)
            {
                Console.WriteLine($"Reason: {reason}");
            }
        }
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        bool verbose = args.Contains("-verbose");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        // Register configuration
        services.AddSingleton(configuration.GetSection("ConnectionStrings").Get<ConnectionStringsConfig>());
        services.AddSingleton(configuration.GetSection("SharedFolders").Get<List<SharedFolderConfig>>());

        // Load assemblies in the same folder
        var assemblyFiles = Directory.GetFiles("HealthChecks", "*.dll");
        foreach (var assemblyFile in assemblyFiles)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            var healthCheckTypes = assembly.GetTypes()
                .Where(type => typeof(IHealthCheck).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var healthCheckType in healthCheckTypes)
            {
                services.AddTransient(typeof(IHealthCheck), healthCheckType);
            }
        }

        // Add HealthCheckManager and ServiceProvider to the service container
        services.AddTransient<HealthCheckManager>();
        var serviceProvider = services.BuildServiceProvider();

        // Run health checks
        var healthCheckManager = serviceProvider.GetRequiredService<HealthCheckManager>();
        healthCheckManager.RunHealthChecks();
    }
}

public class ConnectionStringsConfig
{
    public string DatabaseConnection { get; set; }
}
