using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Threading.RateLimiting;
using UserManagementAPI;
using UserManagementAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Management API",
        Version = "v1",
        Description = "An API for managing users in TechHive Solutions."
    });
});

// Add logging
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.WriteTo.Console());

// Add an in-memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("UserManagementDb"));

// Add Rate Limiter service
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10, // Limit to 10 requests per minute
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5 // Allow up to 5 queued requests
            }));
});

var app = builder.Build();

// Seed data into the in-memory database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Users.Any())
    {
        context.Users.AddRange(new List<User>
        {
            new User { Name = "Alice", Email = "alice@example.com", Role = "Admin" },
            new User { Name = "Bob", Email = "bob@example.com", Role = "User" }
        });
        context.SaveChanges();
    }
}

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1"));
}

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Apply exception handling middleware
app.UseHttpsRedirection();
app.UseRateLimiter(); // Apply rate limiting middleware
app.UseAuthorization();
app.MapControllers();

app.Run();
