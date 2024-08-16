namespace DotnetHeadStart.API;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        //Add services to the DI container
        return builder;
    }
}
