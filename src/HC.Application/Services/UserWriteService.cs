using HC.Application.Common.Constants;
using HC.Application.Interface;
using HC.Application.Interface.Generators;
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
    private readonly IIdGenerator _idGenerator;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public UserWriteService(
        IUserWriteRepository repository,
        JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters,
        IIdGenerator idGenerator)
    {
        _repository = repository;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _idGenerator = idGenerator;
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

    public async Task<BaseResult> PublishReview(PublishReviewCommand command)
    {
        User? user = await _repository.GetUserById(new UserId(command.ReviewerId));

        if (user is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        if (command.ReviewId.HasValue)
        {
            user.RePublishReview(
                new UserId(command.PublisherId),
                command.Message,
                new ReviewId(command.ReviewId.Value));
        }
        else
        {
            user.PublishNewReview(
                new UserId(command.PublisherId),
                command.Message,
                _idGenerator.Generate((id) => new ReviewId(id)));
        }

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

    public async Task<UserWithTokenResult> LoginUser(LoginUserCommand command)
    {
        User? user = await _repository.GetUserByUsername(command.Username);

        if (user is null)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        // TODO: add a strong salt
        var passwordMatch = HashPassword(command.Password, "123") == user.Password;

        if (passwordMatch is false)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.PasswordMismatch);
        }

        if (user.Banned)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserIsBanned);
        }

        (string generatedToken, RefreshToken generatedRefreshToken) = await GenerateJwtToken(user);

        if (string.IsNullOrEmpty(generatedToken) || string.IsNullOrEmpty(generatedRefreshToken.Token))
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.TryAgainLater);
        }

        user.UpdateRefreshToken(generatedRefreshToken);

        return UserWithTokenResult.CreateSuccess(generatedToken, generatedRefreshToken.Token);
    }

    public async Task<UserWithTokenResult> RegisterUser(RegisterUserCommand command)
    {
        User? user = await _repository.GetUserByUsername(command.Username);

        if (user is not null)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserWithUsernameExists);
        }

        var exists = await _repository.IsUserExistByEmail(command.Email);

        if (exists)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserWithEmailExists);
        }

        // TODO: add a strong salt
        string encryptedpassword = HashPassword(command.Password, "123");

        var userId = _idGenerator.Generate((id) => new UserId(id));
        var createdUser = new User(
            userId,
            command.Username,
            command.Email,
            encryptedpassword,
            DateTime.UtcNow,
            DateTime.UtcNow
            );

        (string generatedToken, RefreshToken generatedRefreshToken) = await GenerateJwtToken(createdUser);

        if (string.IsNullOrEmpty(generatedToken) || string.IsNullOrEmpty(generatedRefreshToken.Token))
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.TryAgainLater);
        }

        createdUser.UpdateRefreshToken(generatedRefreshToken);

        await _repository.AddUser(createdUser);

        return UserWithTokenResult.CreateSuccess(generatedToken, generatedRefreshToken.Token);
    }

    public Task<RefreshTokenResponse> RefreshToken(RefreshTokenCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> UpdateUserData(UpdateUserDataCommand command)
    {
        throw new System.NotImplementedException();
    }

    private ClaimsPrincipal? GetClaimsPrincipalFromToken(string token)
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

    private async Task<(string, RefreshToken)> GenerateJwtToken(User user)
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

        return (tokenHandler.WriteToken(token), refreshToken);
    }

    private static string HashPassword(string password, string salt)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }
}
