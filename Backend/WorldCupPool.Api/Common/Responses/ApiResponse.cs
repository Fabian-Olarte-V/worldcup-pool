namespace WorldCupPool.Api.Common.Responses
{
    public sealed class ApiResponse
    {
        public bool Success { get; init; }
        public int StatusCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public object? Data { get; init; }
        public IReadOnlyList<string> Errors { get; init; } = [];
        public string TraceId { get; init; } = string.Empty;
        public DateTime TimestampUtc { get; init; }

        public static ApiResponse FromSuccess(int statusCode, object? data, string traceId, string message = "Request completed successfully.")
        {
            return new ApiResponse
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = [],
                TraceId = traceId,
                TimestampUtc = DateTime.UtcNow
            };
        }

        public static ApiResponse FromError(int statusCode, string message, IReadOnlyList<string> errors, string traceId)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Data = null,
                Errors = errors,
                TraceId = traceId,
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}
