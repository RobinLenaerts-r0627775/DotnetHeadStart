namespace DotnetHeadStart;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Add Serilog configuration. 
    /// To enable file logging enter a valid file path to HeadStart:Logging in user secrets or HeadStart__Logging in .env. json format in appsettings.json
    /// Logger will be available as a singleton in the DI container and staticly in the Log class.
    /// <summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns>the builder for builder pattern functionality</returns>
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        //Configure serilog
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:dd MM yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
        if (!string.IsNullOrWhiteSpace(configuration.GetSection("HeadStart")?.GetSection("Logging")?.Value))
        {
            loggerConfig = loggerConfig.WriteTo.File(configuration.GetSection("HeadStart").GetSection("Logging").Value + "/HeadStartLog.log", outputTemplate:
                "[{Timestamp:dd MM yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day);
        }
        var logger = loggerConfig.CreateLogger();
        builder.Services.AddSingleton<ILogger>(logger);
        Log.Logger = logger;
        if (string.IsNullOrWhiteSpace(configuration.GetSection("HeadStart").GetSection("Logging").Value))
        {
            Log.Warning("No logging file location specified. Logging to console only.(HeadStart:Logging in user secrets or HeadStart__Logging in .env. json format in appsettings.json)");
        }
        else
        {
            Log.Information("Logging to file: " + configuration.GetSection("HeadStart").GetSection("Logging").Value + "/HeadStartLog.log");
        }
        return builder;
    }
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        return builder.ConfigureLogging(builder.Configuration);
    }

    public static WebApplicationBuilder ConfigureMailing(this WebApplicationBuilder builder)
    {
        //Configure mailing
        builder.Services.AddSingleton<IMailSender, SMTPMailSender>();
        return builder;
    }
}