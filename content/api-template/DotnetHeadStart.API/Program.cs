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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultEndpoints(app.Configuration);
app.MapHealthChecks("/health");
app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.Run();
