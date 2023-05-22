//Configure configmanager
var configManager = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
var builder = WebApplication.CreateBuilder(args);

//Configure serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
if (!string.IsNullOrWhiteSpace(configManager.GetSection("FileLocations").GetSection("Logging").Value))
{
    loggerConfig = loggerConfig.WriteTo.File(configManager.GetSection("FileLocations").GetSection("Logging").Value + "/HeadStartLog.log", outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day);
}
var logger = loggerConfig.CreateLogger();
builder.Services.AddSingleton<ILogger>(logger);

logger.Information("Starting up");

// Configure database mysql context
var connectionstring = configManager.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HeadStartContext>(options =>
    options.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring)));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Swagger config
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

// Minimal API 
#region Minimal API
app.MapGet("/api/ping", () => "pong");
var appversion = configManager.GetSection("AppVersion").Value;
app.MapGet("/api/version", () => appversion);
#endregion

app.Run();
