using System.Text.Json;
using WorldCupPool.Api.Exceptions;
using WorldCupPool.Api.Common.Responses;
using WorldCupPool.Application.Exceptions;

namespace WorldCupPool.Api.Middlewares
{
    public sealed class GlobalExceptionMiddleware : IMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                throw exception;
            }

            var statusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                AuthenticationException => StatusCodes.Status401Unauthorized,
                RequestContextException => StatusCodes.Status401Unauthorized,
                NotFoundException => StatusCodes.Status404NotFound,
                BusinessRuleViolationException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = ApiResponse.FromError(
                statusCode,
                GetMessage(statusCode),
                [exception.Message],
                context.TraceIdentifier);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await JsonSerializer.SerializeAsync(context.Response.Body, response, JsonOptions);
        }

        private static string GetMessage(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "The request could not be processed.",
                StatusCodes.Status401Unauthorized => "Authentication is required to access this resource.",
                StatusCodes.Status404NotFound => "The requested resource was not found.",
                StatusCodes.Status409Conflict => "The request violates a business rule.",
                _ => "An unexpected error occurred."
            };
        }
    }
}
