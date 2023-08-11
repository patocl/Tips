using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class DependencyConfigurator
{
    public static void Configure(IServiceCollection services)
    {
        // Configure your dependencies here
        // ...
    }
}

public interface IRunner
{
    void Run();
}

public interface IHealthCheck
{
    void Check();
}

public interface IConfigurationValidator
{
    void Validate();
}

public class RunnerExecutor
{
    private readonly IServiceProvider _serviceProvider;

    public RunnerExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void RunAllFromAssemblies()
    {
        RunAll<IRunner>("Runner");
        RunAll<IHealthCheck>("HealthCheck");
        RunAll<IConfigurationValidator>("ConfigurationValidator");
    }

    private void RunAll<T>(string suffix) where T : class
    {
        var runnerInterfaceType = typeof(T);
        var types = DiscoverTypesWithSuffix(runnerInterfaceType, suffix);

        foreach (var type in types)
        {
            var instance = ActivatorUtilities.CreateInstance(_serviceProvider, type) as T;
            instance?.Run();
        }
    }

    private IEnumerable<Type> DiscoverTypesWithSuffix(Type targetType, string suffix)
    {
        var targetTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsInterface && !t.IsAbstract && targetType.IsAssignableFrom(t) && t.Name.EndsWith(suffix));

        return targetTypes;
    }
}

public class HealthCheckRunner : IRunner
{
    public void Run()
    {
        Console.WriteLine("Running Health Check Runner...");
    }
}

public class DatabaseHealthCheck : IHealthCheck
{
    public void Check()
    {
        Console.WriteLine("Running Database Health Check...");
    }
}

public class ConfigurationValidatorRunner : IRunner
{
    public void Run()
    {
        Console.WriteLine("Running Configuration Validator Runner...");
    }
}

public class DatabaseConfigurationValidator : IConfigurationValidator
{
    public void Validate()
    {
        Console.WriteLine("Running Database Configuration Validator...");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        DependencyConfigurator.Configure(services);

        // Register the RunnerExecutor
        services.AddSingleton<RunnerExecutor>();

        var serviceProvider = services.BuildServiceProvider();
        var runnerExecutor = serviceProvider.GetRequiredService<RunnerExecutor>();

        // Execute all runners from assemblies
        runnerExecutor.RunAllFromAssemblies();
    }
}
