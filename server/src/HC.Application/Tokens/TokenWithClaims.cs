using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

public sealed class TokenWithClaims
{
    private readonly ClaimsPrincipal _principal;

    public TokenWithClaims(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        _principal = principal;
    }

    public DateTime ExpiryDate =>
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(
                    long.Parse(_principal.Claims.Single(
                        x => x.Type == JwtRegisteredClaimNames.Exp).Value));

    public string JTI => _principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
}