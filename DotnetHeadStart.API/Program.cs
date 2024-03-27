var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.ConfigureLogging();

Log.Error("Starting application");

var app = builder.Build();

//catch global error handling
app.UseExceptionHandler("/error"); // Replace "/error" with your desired error handling endpoint

// error enpoint
app.MapGet("/error", (ILogger logger, Exception e) =>
{
    logger.Error("An error occurred");
    return Results.BadRequest($"An error occurred: {e.Message}");
});

app.MapGet("/test", (e) => throw new NotImplementedException("Not implemented yet"));


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.MapControllers();

app.UseHttpsRedirection();
app.Run();
