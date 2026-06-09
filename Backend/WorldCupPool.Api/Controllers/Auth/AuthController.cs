using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldCupPool.Application.Features.Auth;
using WorldCupPool.Application.Features.Auth.DTOs.Requests;
using WorldCupPool.Application.Features.Auth.DTOs.Responses;

namespace WorldCupPool.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto req, CancellationToken ct)
        {
            var result = await _authService.Login(req, ct);
            return Ok(result);
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponseDto>> SignUp(RegisterRequestDto req, CancellationToken ct)
        {
            var result = await _authService.SignUp(req, ct);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenRequestDto req, CancellationToken ct)
        {
            var result = await _authService.RefreshToken(req, ct);
            return Ok(result);
        }
    }
}
