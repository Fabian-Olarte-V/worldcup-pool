namespace WorldCupPool.Application.Features.Auth.DTOs.Responses
{
    public sealed class AuthResponseDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}

