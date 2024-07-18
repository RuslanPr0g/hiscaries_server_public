using HC.Application.Common.Constants;
using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Application.Options;
using HC.Application.Users.Command;
using HC.Application.Users.Command.LoginUser;
using HC.Application.Users.Command.PublishReview;
using HC.Application.Users.Command.RefreshToken;
using HC.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HC.Application.Services;

public sealed class UserWriteService : IUserWriteService
{
    private readonly IUserWriteRepository _repository;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public UserWriteService(
        IUserWriteRepository repository,
        JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters)
    {
        _repository = repository;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<BaseResult> BecomePublisher(string username)
    {
        User? user = await _repository.GetUserByUsername(username);

        if (user is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        user.BecomePublisher();

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> DeleteReview(DeleteReviewCommand command)
    {
        User? user = await _repository.GetUserByUsername(command.Username);

        if (user is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        user.RemoveReview(new ReviewId(command.ReviewId));

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult<User>> GetUserById(UserId userId)
    {
        User? user = await _repository.GetUserById(userId);

        if (user is null)
        {
            return BaseResult<User>.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        return BaseResult<User>.CreateSuccess(user);
    }

    public async Task<BaseResult<User>> GetUserByUsername(string username)
    {
        User? user = await _repository.GetUserByUsername(username);

        if (user is null)
        {
            return BaseResult<User>.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        return BaseResult<User>.CreateSuccess(user);
    }

    public Task<LoginUserResult> LoginUser(LoginUserCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> PublishReview(PublishReviewCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<RefreshTokenResponse> RefreshToken(RefreshTokenCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<UserWithTokenResult> RegisterUser(RegisterUserCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> UpdateUserData(UpdateUserDataCommand command)
    {
        throw new System.NotImplementedException();
    }

    private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        try
        {
            ClaimsPrincipal principal =
                tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);

            return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    private async Task<(string, string)> GenerateJwtToken(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        string jti = Guid.NewGuid().ToString();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, jti),
                    new Claim(JwtRegisteredClaimNames.Email, user.Username),
                    new Claim("id", user.Id.Value.ToString()),
                    new Claim("username", user.Username)
                }),
            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        RefreshToken refreshToken = new(
            new RefreshTokenId(Guid.NewGuid()),
            token.Id,
            jti,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(6),
            false,
            false);

        await _repository.UpdateRefreshToken(refreshToken);
        return (tokenHandler.WriteToken(token), refreshToken.Token);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
