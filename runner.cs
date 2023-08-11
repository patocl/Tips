using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Linq;
using System.Reflection;

public abstract class RunnerBase
{
    protected IServiceProvider ServiceProvider { get; }

    protected RunnerBase()
    {
        // Setup the service collection and configure dependencies
        var services = new ServiceCollection();
        ConfigureDependencies(services);

        // Build the service provider
        ServiceProvider = services.BuildServiceProvider();
    }

    protected abstract void ConfigureDependencies(IServiceCollection services);

    protected T ResolveService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    public abstract void Run();
}

public class HealthCheckRunner : RunnerBase
{
    protected override void ConfigureDependencies(IServiceCollection services)
    {
        // Configure dependencies here using DependencyConfigurator
        DependencyConfigurator.Configure(services);
    }

    public override void Run()
    {
        // Resolve and execute Health Checks
        var healthChecks = ServiceProvider.GetServices<IHealthCheck>();
        foreach (var healthCheck in healthChecks)
        {
            var result = healthCheck.CheckHealthAsync(new HealthCheckContext()).GetAwaiter().GetResult();
            Console.WriteLine($"Health Check {healthCheck.GetType().Name}: {result.Status}");
        }
    }
}

public class ConfigurationValidatorRunner : RunnerBase
{
    protected override void ConfigureDependencies(IServiceCollection services)
    {
        // Configure dependencies here using DependencyConfigurator
        DependencyConfigurator.Configure(services);
    }

    public override void Run()
    {
        // Resolve and execute Configuration Validators
        var configValidators = ServiceProvider.GetServices<ConfigurationValidatorBase>();
        foreach (var configValidator in configValidators)
        {
            configValidator.Validate();
            Console.WriteLine($"Configuration Validator {configValidator.GetType().Name}: Validated");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Usage examples
        var healthCheckRunner = new HealthCheckRunner();
        healthCheckRunner.Run();

        var configValidatorRunner = new ConfigurationValidatorRunner();
        configValidatorRunner.Run();
    }
}
