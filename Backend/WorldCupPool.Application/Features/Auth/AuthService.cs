using WorldCupPool.Application.Common;
using WorldCupPool.Application.Exceptions;
using WorldCupPool.Application.Features.Auth.DTOs.Requests;
using WorldCupPool.Application.Features.Auth.DTOs.Responses;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Domain.Enums;
using System.Security.Cryptography;

namespace WorldCupPool.Application.Features.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IAppUserRepository appUserRepository, IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher)
        {
            _appUserRepository = appUserRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }


        public async Task<AuthResponseDto> Login(LoginRequestDto request, CancellationToken ct)
        {
            var user = await _appUserRepository.GetByUserNameAsync(request.Username, ct)
                ?? await _appUserRepository.GetByEmailAsync(request.Username, ct)
                ?? throw new AuthenticationException("Invalid credentials.");

            var passwordHashValidation = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!passwordHashValidation)
            {
                throw new AuthenticationException("Invalid credentials.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Role.ToString());
            var refreshToken = GenerateRefreshToken();

            user.SetRefreshToken(refreshToken);
            await _appUserRepository.SaveChangesAsync(ct);

            return BuildResponse(user, token, refreshToken);
        }

        public async Task<AuthResponseDto> SignUp(RegisterRequestDto request, CancellationToken ct)
        {
            var passwordHash = _passwordHasher.HashPassword(request.Password);
            var role = ParseUserRole(request.Role);
            var user = new AppUser(request.FirstName, request.LastName, request.Email,
                request.Username, passwordHash, role);

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Role.ToString());
            var refreshToken = GenerateRefreshToken();

            user.SetRefreshToken(refreshToken);

            await _appUserRepository.CreateAsync(user, ct);
            await _appUserRepository.SaveChangesAsync(ct);

            return BuildResponse(user, token, refreshToken);
        }

        public async Task<AuthResponseDto> RefreshToken(RefreshTokenRequestDto request, CancellationToken ct)
        {
            var user = await _appUserRepository.GetByRefreshTokenAsync(request.RefreshToken, ct)
                ?? throw new AuthenticationException("Refresh token was not found.");

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Role.ToString());
            var refreshToken = GenerateRefreshToken();

            user.SetRefreshToken(refreshToken);
            await _appUserRepository.SaveChangesAsync(ct);

            return BuildResponse(user, token, refreshToken);
        }



        private static string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        private static UserRole ParseUserRole(string role)
        {
            if (Enum.TryParse<UserRole>(role, true, out var parsedRole) &&
                (parsedRole == UserRole.User || parsedRole == UserRole.Admin))
            {
                return parsedRole;
            }

            throw new ValidationException("Role must be ADMIN or USER.");
        }

        private static AuthResponseDto BuildResponse(AppUser user, string token, string refreshToken)
        {
            return new AuthResponseDto
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAtUtc = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
