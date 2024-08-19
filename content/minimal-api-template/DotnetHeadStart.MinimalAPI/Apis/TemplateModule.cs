namespace DotnetHeadStart.MinimalAPI.Apis;

public static class TemplateModule
{
    public static WebApplication RegisterTemplateModule(this WebApplication app)
    {
        var template = app.MapGroup("/template");
        template.MapGet("/", HelloWorld);
        template.MapGet("/error", Error);
        return app;
    }

    private static async Task<IResult> HelloWorld()
    {
        return Results.Ok(await Task.FromResult("Hello World"));
    }

    private static Task<IResult> Error()
    {
        throw new Exception("An error occurred");
    }
}
