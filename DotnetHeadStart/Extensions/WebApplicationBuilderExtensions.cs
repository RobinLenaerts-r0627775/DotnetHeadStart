namespace DotnetHeadStart;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the HeadStart services to the WebApplicationBuilder
    /// list of services:
    /// - Serilog file and console logger
    /// <summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns>the builder for builder pattern functionality</returns>
    public static WebApplicationBuilder AddHeadStartLogging(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        //Configure serilog
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:dd MM yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
        if (!string.IsNullOrWhiteSpace(configuration.GetSection("FileLocations")?.GetSection("Logging")?.Value))
        {
            loggerConfig = loggerConfig.WriteTo.File(configuration.GetSection("FileLocations").GetSection("Logging").Value + "/HeadStartLog.log", outputTemplate:
                "[{Timestamp:dd MM yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day);
        }
        var logger = loggerConfig.CreateLogger();
        builder.Services.AddSingleton<ILogger>(logger);
        Log.Logger = logger;
        if (string.IsNullOrWhiteSpace(configuration.GetSection("FileLocations").GetSection("Logging").Value))
        {
            Log.Warning("No logging file location specified. Logging to console only.(FileLocations:Logging in user secrets or FileLocations__Logging in .env. json format in appsettings.json)");
        }
        else
        {
            Log.Information("Logging to file: " + configuration.GetSection("FileLocations").GetSection("Logging").Value + "/HeadStartLog.log");
        }
        return builder;
    }
}