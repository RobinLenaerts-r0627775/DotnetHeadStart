//configure configmanager
var configManager = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

//configure serilog
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
if (!string.IsNullOrWhiteSpace(configManager.GetSection("FileLocations").GetSection("Logging").Value))
{
    loggerConfig = loggerConfig.WriteTo.File(configManager.GetSection("FileLocations").GetSection("Logging").Value + "/test.log", rollingInterval: RollingInterval.Day, outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
}
var logger = loggerConfig.CreateLogger();
builder.Services.AddSingleton<ILogger>(logger);

logger.Information("Starting up");

// add database mysql context
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseMySql(configManager.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configManager.GetConnectionString("DefaultConnection"))));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.Run();
