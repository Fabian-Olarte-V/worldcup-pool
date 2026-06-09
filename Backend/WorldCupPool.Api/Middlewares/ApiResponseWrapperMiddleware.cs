using System.Text.Json;
using WorldCupPool.Api.Common.Responses;

namespace WorldCupPool.Api.Middlewares
{
    public sealed class ApiResponseWrapperMiddleware : IMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;
            await using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await next(context);

            if (context.Response.HasStarted)
            {
                return;
            }

            var statusCode = context.Response.StatusCode;
            var contentType = context.Response.ContentType;

            if (statusCode == StatusCodes.Status204NoContent)
            {
                context.Response.Body = originalBodyStream;
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";

                var noContentResponse = ApiResponse.FromSuccess(
                    StatusCodes.Status200OK,
                    null,
                    context.TraceIdentifier,
                    "Request completed successfully.");

                await JsonSerializer.SerializeAsync(context.Response.Body, noContentResponse, JsonOptions);
                return;
            }

            responseBodyStream.Position = 0;
            var body = await new StreamReader(responseBodyStream).ReadToEndAsync();

            context.Response.Body = originalBodyStream;

            if (!ShouldWrap(contentType, body))
            {
                if (!string.IsNullOrEmpty(contentType))
                {
                    context.Response.ContentType = contentType;
                }

                await context.Response.WriteAsync(body);
                return;
            }

            var data = TryDeserializeBody(body);
            var wrappedResponse = ApiResponse.FromSuccess(
                statusCode,
                data,
                context.TraceIdentifier,
                "Request completed successfully.");

            context.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(context.Response.Body, wrappedResponse, JsonOptions);
        }

        private static bool ShouldWrap(string? contentType, string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                return true;
            }

            return contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase) ||
                   contentType.Contains("text/plain", StringComparison.OrdinalIgnoreCase);
        }

        private static object? TryDeserializeBody(string body)
        {
            try
            {
                using var document = JsonDocument.Parse(body);
                return document.RootElement.Clone();
            }
            catch (JsonException)
            {
                return body;
            }
        }
    }
}
