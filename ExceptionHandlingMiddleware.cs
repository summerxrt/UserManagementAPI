using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace UserManagementAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Handle 404 Not Found
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await HandleNotFoundAsync(context);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred.",
                Details = exception.Message // Avoid exposing sensitive details in production
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleNotFoundAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Resource not found."
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
