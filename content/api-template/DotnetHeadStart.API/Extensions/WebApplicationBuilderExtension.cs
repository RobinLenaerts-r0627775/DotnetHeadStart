using Microsoft.EntityFrameworkCore;

namespace DotnetHeadStart.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        //Add services to the DI container
        return builder;
    }
    /// <summary>
    /// Configure MySql database connection based on environment variables: DB_HOSTNAME, DB_PORT, DB_USER, DB_PASSWORD, DB_NAME
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="builder"></param>
    /// <param name="UseConnectionVariables">If true, the connection string will be built using environment variables. If false, the connection string will be built using the DefaultConnection key in the appsettings.json file.</param>
    /// <returns>Builder for chaining.</returns>
    public static WebApplicationBuilder ConfigureMySqlDatabaseconnection<T>(this WebApplicationBuilder builder, bool UseConnectionVariables = false) where T : DbContext
    {
        var configManager = builder.Configuration;
        string connectionstring;
        if (UseConnectionVariables)
        {
            // Configure database mysql context
            var hostname = configManager["DB_HOSTNAME"];
            if (string.IsNullOrWhiteSpace(hostname))
            {
                Log.Fatal("DB_HOSTNAME is not set. Please set it in the environment variables.");
                // throw new HeadStartException("DB_HOSTNAME is not set");
            }
            var port = configManager["DB_PORT"];
            if (string.IsNullOrWhiteSpace(port))
            {
                Log.Information("DB_PORT not provided, using default 3306");
                port = "3306";
            }
            var user = configManager["DB_USER"];
            if (string.IsNullOrWhiteSpace(user))
            {
                Log.Fatal("DB_USER is not set. Please set it in the environment variables.");
                // throw new HeadStartException("DB_USER is not set");
            }
            var password = configManager["DB_PASSWORD"];
            if (string.IsNullOrWhiteSpace(password))
            {
                Log.Fatal("DB_PASSWORD is not set. Please set it in the environment variables.");
                // throw new HeadStartException("DB_PASSWORD is not set");
            }
            var databaseName = configManager["DB_NAME"];
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                Log.Fatal("DB_NAME is not set. Please set it in the environment variables.");
                // throw new HeadStartException("DB_NAME is not set");
            };

            connectionstring = $"server={hostname};port={port};database={databaseName};user={user};password={password};convert zero datetime=True;Keepalive=120"; // mysql connection string
        }
        else
        {
            connectionstring = configManager.GetConnectionString("DefaultConnection") ?? "";
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                Log.Fatal("DefaultConnection is not set. Please set it in the environment variables under key: ConnectionStrings > DefaultConnection");
                throw new HeadStartException("DefaultConnection is not set");
            }
        }
        builder.Services.AddDbContext<T>(options =>
            {
                options.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring));
            });

        return builder;
    }
}
