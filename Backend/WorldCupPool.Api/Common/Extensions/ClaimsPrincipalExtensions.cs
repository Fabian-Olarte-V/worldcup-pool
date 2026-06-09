using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorldCupPool.Api.Exceptions;

namespace WorldCupPool.Api.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new RequestContextException("Authenticated user id was not found.");

            return Guid.Parse(userId);
        }
    }
}
