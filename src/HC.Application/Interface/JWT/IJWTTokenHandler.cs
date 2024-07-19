using HC.Application.Services;
using HC.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace HC.Application.Interface.JWT;

public interface IJWTTokenHandler
{
    TokenWithClaims? GetTokenWithClaims(string token, TokenValidationParameters parameters);
    Task<(string AccessKey, RefreshTokenDescriptor ReshreshToken)> GenerateJwtToken(User user, string key, TimeSpan lifetime);
}
