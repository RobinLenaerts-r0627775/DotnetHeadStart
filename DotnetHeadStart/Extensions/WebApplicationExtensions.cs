namespace DotnetHeadStart;

public static class WebApplicationExtensions
{
    public static WebApplication UseHeadStart(this WebApplication app, IConfiguration configuration)
    {
        // Minimal API 
        #region Minimal API
        app.MapGet("/api/ping", () => "pong");
        app.MapGet("/api/version", () => configuration["Version"] ?? "No version specified (you can specify a version in a configuration variable called 'Version')");
        #endregion

        return app;
    }
}
