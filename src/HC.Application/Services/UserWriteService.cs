using HC.Application.Common.Constants;
using HC.Application.Interface;
using HC.Application.Interface.Generators;
using HC.Application.Interface.JWT;
using HC.Application.Models.Response;
using HC.Application.Options;
using HC.Application.Users.Command;
using HC.Application.Users.Command.LoginUser;
using HC.Application.Users.Command.PublishReview;
using HC.Application.Users.Command.RefreshToken;
using HC.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace HC.Application.Services;

public sealed class UserWriteService : IUserWriteService
{
    private readonly IUserWriteRepository _repository;
    private readonly IIdGenerator _idGenerator;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IJWTTokenHandler _tokenHandler;

    public UserWriteService(
        IUserWriteRepository repository,
        JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters,
        IIdGenerator idGenerator,
        IJWTTokenHandler tokenHandler)
    {
        _repository = repository;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _idGenerator = idGenerator;
        _tokenHandler = tokenHandler;
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

        (string generatedToken, RefreshTokenDescriptor generatedRefreshToken) =
            await _tokenHandler.GenerateJwtToken(user, _jwtSettings.Key, _jwtSettings.TokenLifeTime);

        if (string.IsNullOrEmpty(generatedToken) || string.IsNullOrEmpty(generatedRefreshToken.Token))
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.TryAgainLater);
        }

        user.UpdateRefreshToken(
            generatedRefreshToken.ToDomainModel(
                _idGenerator.Generate((id) => new RefreshTokenId(id))));

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

        (string generatedToken, RefreshTokenDescriptor generatedRefreshToken) =
            await _tokenHandler.GenerateJwtToken(createdUser, _jwtSettings.Key, _jwtSettings.TokenLifeTime);

        if (string.IsNullOrEmpty(generatedToken) || string.IsNullOrEmpty(generatedRefreshToken.Token))
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.TryAgainLater);
        }

        createdUser.UpdateRefreshToken(
            generatedRefreshToken.ToDomainModel(
                _idGenerator.Generate((id) => new RefreshTokenId(id))));

        await _repository.AddUser(createdUser);

        return UserWithTokenResult.CreateSuccess(generatedToken, generatedRefreshToken.Token);
    }

    public async Task<UserWithTokenResult> RefreshToken(RefreshTokenCommand command)
    {
        User? user = await _repository.GetUserByUsername(command.Username);

        if (user is null)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        var validatedToken = _tokenHandler.GetTokenWithClaims(command.Token, _tokenValidationParameters);

        if (validatedToken is null)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.PleaseRelogin);
        }

        if (validatedToken.ExpiryDate > DateTime.UtcNow)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.RefreshTokenIsNotExpired);
        }

        bool validated = user.ValidateRefreshToken(validatedToken.JTI);

        if (!validated)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.RefreshTokenIsExpired);
        }

        (string generatedToken, RefreshTokenDescriptor generatedRefreshToken) =
            await _tokenHandler.GenerateJwtToken(user, _jwtSettings.Key, _jwtSettings.TokenLifeTime);

        if (string.IsNullOrEmpty(generatedToken) || string.IsNullOrEmpty(generatedRefreshToken.Token))
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.TryAgainLater);
        }

        user.UpdateRefreshToken(
            generatedRefreshToken.ToDomainModel(
                _idGenerator.Generate((id) => new RefreshTokenId(id))));

        return new UserWithTokenResult(ResultStatus.Success, string.Empty, generatedToken, generatedRefreshToken.Token);
    }

    public async Task<BaseResult> UpdateUserData(UpdateUserDataCommand command)
    {
        User? user = await _repository.GetUserByUsername(command.Username);

        if (user is null)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserIsNotFound);
        }

        if (!string.IsNullOrEmpty(command.UpdatedUsername))
        {
            var newUsernameUser = await _repository.GetUserByUsername(command.UpdatedUsername);

            if (newUsernameUser is not null)
            {
                return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserWithUsernameExists);
            }
        }

        var existsWithEmail = await _repository.IsUserExistByEmail(command.Email);

        if (existsWithEmail)
        {
            return UserWithTokenResult.CreateFail(UserFriendlyMessages.UserWithEmailExists);
        }

        if (!string.IsNullOrEmpty(command.NewPassword))
        {
            var result = UpdateUserPassword(user, command.PreviousPassword, command.NewPassword);

            if (result.ResultStatus is ResultStatus.Success)
            {
                user.UpdatePersonalInformation(command.UpdatedUsername, command.Email, command.BirthDate);
            }

            return result;
        }
        else
        {
            user.UpdatePersonalInformation(command.UpdatedUsername, command.Email, command.BirthDate);
        }

        return BaseResult.CreateSuccess();
    }

    private BaseResult UpdateUserPassword(User user, string previousPassword, string newPassword)
    {
        var hashedPreviousPassword = HashPassword(previousPassword, "123");

        if (user.Password != hashedPreviousPassword)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.PasswordMismatch);
        }

        var hashedNewPassword = HashPassword(newPassword, "123");

        user.UpdatePassword(hashedNewPassword);

        return BaseResult.CreateSuccess();
    }

    private static string HashPassword(string password, string salt)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }
}
