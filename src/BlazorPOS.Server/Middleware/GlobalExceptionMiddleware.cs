using System.Net;
using System.Text.Json;
using BlazorPOS.Shared.Exceptions;

namespace BlazorPOS.Server.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                ValidationException validationEx => (HttpStatusCode.BadRequest, new { Errors = validationEx.Errors }),
                InsufficientInventoryException inventoryEx => (HttpStatusCode.BadRequest, new 
                { 
                    Message = inventoryEx.Message,
                    ProductId = inventoryEx.ProductId,
                    RequestedQuantity = inventoryEx.RequestedQuantity,
                    AvailableQuantity = inventoryEx.AvailableQuantity
                }),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            context.Response.StatusCode = (int)response.Item1;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response.Item2));
        }
    }