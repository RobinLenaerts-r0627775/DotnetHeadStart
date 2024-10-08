﻿namespace DotnetHeadStart.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Maps default endpoints to the application:
    /// - /api/ping: returns "pong"
    /// - /api/version: returns the version of the application
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static WebApplication UseDefaultEndpoints(this WebApplication app, IConfiguration? configuration = null)
    {
        configuration ??= app.Configuration;
        // Minimal API 
        #region Minimal API
        app.MapGet("/api/ping", () => "pong");
        app.MapGet("/api/version", () => configuration.GetSection("HeadStart").GetSection("Version").Value ?? "No version specified (you can specify a version in a configuration variable called 'Version' under the HeadStart section)");
        #endregion
        return app;
    }

    /// <summary>
    /// Maps default endpoints to the application:
    /// - /api/ping: returns "pong"
    /// - /api/version: returns the version of the application
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseDefaultEndpoints(this WebApplication app)
    {
        return app.UseDefaultEndpoints(app.Configuration);
    }
}
