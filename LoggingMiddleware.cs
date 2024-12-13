using System.Diagnostics;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Incoming Request: {Method} {Path}",
            context.Request.Method, context.Request.Path);

        var stopwatch = Stopwatch.StartNew();
        await _next(context); // Call the next middleware
        stopwatch.Stop();

        _logger.LogInformation("Outgoing Response: {StatusCode} - Time: {ElapsedMilliseconds}ms",
            context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}
