using WorldCupPool.Application.Features.Auth.DTOs.Requests;
using WorldCupPool.Application.Features.Auth.DTOs.Responses;

namespace WorldCupPool.Application.Features.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginRequestDto request, CancellationToken ct);
        Task<AuthResponseDto> SignUp(RegisterRequestDto request, CancellationToken ct);
        Task<AuthResponseDto> RefreshToken(RefreshTokenRequestDto request, CancellationToken ct);
    }
}
