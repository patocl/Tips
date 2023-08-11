using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public abstract class ConfigurationValidatorBase
{
    protected bool IsValid;
    protected string ErrorMessage;

    public bool Validate(out string errorMessage)
    {
        ValidateConfiguration(out errorMessage);
        return IsValid;
    }

    protected abstract void ValidateConfiguration(out string errorMessage);
}

// DatabaseConfigurationValidator and SharedFolderConfigurationValidator classes remain the same

public class SharedFolderConfig
{
    public string Path { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class ConfigurationValidatorManager
{
    private readonly IEnumerable<ConfigurationValidatorBase> _validators;

    public ConfigurationValidatorManager(IEnumerable<ConfigurationValidatorBase> validators)
    {
        _validators = validators;
    }

    public void ValidateAll()
    {
        foreach (var validator in _validators)
        {
            string errorMessage;
            bool isValid = validator.Validate(out errorMessage);

            Console.WriteLine($"Configuration Validation '{validator.GetType().Name}': {(isValid ? "Valid" : "Invalid")}");
            if (!isValid)
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        // Register configuration
        services.AddSingleton(configuration.GetSection("ConnectionStrings").Get<ConnectionStringsConfig>());
        services.AddSingleton(configuration.GetSection("SharedFolders").Get<List<SharedFolderConfig>>());

        // Load assemblies in the same folder
        var assemblyFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
        foreach (var assemblyFile in assemblyFiles)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            var validatorTypes = assembly.GetTypes()
                .Where(type => typeof(ConfigurationValidatorBase).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var validatorType in validatorTypes)
            {
                services.AddTransient(typeof(ConfigurationValidatorBase), validatorType);
            }
        }

        // Add ConfigurationValidatorManager to the service container
        services.AddTransient<ConfigurationValidatorManager>();
        var serviceProvider = services.BuildServiceProvider();

        // Run configuration validations
        var validatorManager = serviceProvider.GetRequiredService<ConfigurationValidatorManager>();
        validatorManager.ValidateAll();
    }
}

public class ConnectionStringsConfig
{
    public string DatabaseConnection { get; set; }
}
