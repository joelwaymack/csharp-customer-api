using CustomerApi.DataAccess;
using CustomerApi.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Retrieve config values
var databaseConnectionString = configuration.GetValue<string>("Database:ConnectionString");
if (string.IsNullOrWhiteSpace(databaseConnectionString))
{
    throw new InvalidOperationException("Database connection string is required");
}

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
services.AddDbContext<CustomerContext>(options =>
{
    options.UseSqlServer(databaseConnectionString);
});

// Run database migrations
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
using var scope = services.BuildServiceProvider().CreateScope();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var context = scope.ServiceProvider.GetRequiredService<CustomerContext>();
context.Database.Migrate();

// Set up request processing pipeline
var app = builder.Build();

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
