using Jzor.Configuration;
using Console = System.Console;

// Print header
Console.WriteLine(ConfigHelper.ProductVersion);
var logger = ConfigHelper.ConfigLogger;

try
{
    // Configure virtual filesystem
    var hostFs = ConfigHelper.GetHostFileSystem();

    // Build
    var builder = WebApplication.CreateBuilder()
        // Configuration
        .AddJzorConfiguration(logger, hostFs)
        // Logging
        .ClearLogProviders()
        .AddJzorConsoleLogger()
        // Services
        .AddJzorServices(hostFs)
        ;

    // Run
    builder.Build()
        .UseJzor()
        .Run()
        ;
}
catch (Exception ex)
{
    logger?.LogError(ex, ex.Message);
}