using DotnetHeadStart.API.DB;
using DotnetHeadStart.API.Extensions;
using DotnetHeadStart.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var configManager = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.ConfigureLogging();

Log.Error("Starting application");

builder.ConfigureMySqlDatabaseconnection<TemplateContext>(true);

var app = builder.Build();

//catch global error handling
app.UseExceptionHandler("/error"); // Replace "/error" with your desired error handling endpoint

// error enpoint
app.MapGet("/error", (ILogger logger, HttpContext httpContext) =>
{
    var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
    logger.Error(exception, $"An error occurred: \n {exception?.Message} \n {exception?.StackTrace}");
    return Results.BadRequest($"An error occurred");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultEndpoints(app.Configuration);
app.MapHealthChecks("/health");
app.MapControllers();

//example

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.Run();
