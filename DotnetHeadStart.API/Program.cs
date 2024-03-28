

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

var connectionstring = $"server={hostname};port={port};database=AITClient;user={user};password={password};convert zero datetime=True;Keepalive=120"; // mysql connection string

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

app.UseHttpsRedirection();
app.Run();
