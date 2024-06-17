using CustomerApi.DataAccess;
using CustomerApi.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Retrieve config values
var databaseConnectionString = configuration.GetValue<string>("Database:ConnectionString");

// Set up dependency injection
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Customer API", Version = "v1" });
    c.EnableAnnotations();
});
services.AddControllers();
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

string databaseProvider;
if (!string.IsNullOrWhiteSpace(databaseConnectionString))
{
    databaseProvider = "SqlServer";
    services.AddDbContext<CustomerContext>(options =>
    {
        options.UseSqlServer(databaseConnectionString);
    });
}
else
{
    databaseProvider = "Sqlite";
    services.AddDbContext<CustomerContext>(options =>
    {
        options.UseSqlite("Data Source=customer.db");
    });
}

// Set up the app
var app = builder.Build();

// Apply database migrations
app.Logger.LogInformation($"Using {databaseProvider} database provider");
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<CustomerContext>();
context.Database.Migrate();

// Set up request processing pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapGet("/", () => Results.Extensions.Html(@"<!doctype html>
<html>
    <head>
        <title>Customer API</title>
    </head>
    <body>
        <h1>Customer API</h1>
        <p>Customer API is running!</p>
        <p>The Swagger UI is available at <a href=""/swagger/index.html"">/swagger/index.html</a></p>
    </body>
</html>"));

// Run the app
app.Run();
