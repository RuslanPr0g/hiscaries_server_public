using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HC.Application.Encryption;
using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Application.Options;
using HC.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HC.Application.Services;

public class UserService : IUserWriteService
{
    private readonly DbConnectionRoleCreator _dbConnectionRoleCreator;
    private readonly IEncryptor _encryptor;
    private readonly JwtSettings _jwtSettings;
    private readonly IStoryWriteRepository _storyRepository;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IUserWriteRepository _userRepository;

    public UserService(IUserWriteRepository userRepository, JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters,
        IOptionsSnapshot<DbConnectionRoleCreator> connection, IEncryptor encryptor,
        IStoryWriteRepository storyRepository)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _dbConnectionRoleCreator = connection.Value;
        _encryptor = encryptor;
        _storyRepository = storyRepository;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _userRepository.GetUserByUsername(username);
    }

    public async Task<string> GetUserRoleByUsername(string username)
    {
        return await _userRepository.GetUserRoleByUsername(username);
    }

    public async Task BecomePublisher(string username)
    {
        string userRole = await _userRepository.GetUserRoleByUsername(username);
        if (userRole != "publisher") await _userRepository.BecomePublisher(username);
    }

    public async Task<User> GetUserById(int userId)
    {
        return await _userRepository.GetUserById(userId);
    }

    public async Task<IList<User>> GetAllUsers()
    {
        return await _userRepository.GetUsers();
    }

    public async Task DeleteReview(Review review)
    {
        await _userRepository.DeleteReview(review);
    }

    public async Task<int> PublishReview(Review review)
    {
        await _userRepository.InsertReview(review);
        return 1;
    }

    public async Task<List<Review>> GetAllReviews()
    {
        return await _userRepository.GetReviews();
    }

    public async Task<UpdateUserDataResult> UpdateUserData(User user, string email, bool banned, DateTime birthdate)
    {
        User oldUser = await GetUserByUsername(user.Username);

        if (!string.IsNullOrEmpty(user.Email))
            oldUser.Email = user.Email;

        if (user.Banned != oldUser.Banned)
            oldUser.Banned = user.Banned;

        if (user.BirthDate < DateTime.Now && user.BirthDate > DateTime.Now.AddYears(-100))
            oldUser.BirthDate = user.BirthDate;

        await _userRepository.UpdateUserData(oldUser);
        return new UpdateUserDataResult(ResultStatus.Success, "User updated");
    }

    public async Task<UserWithTokenResult> RegisterUser(User user)
    {
        User userExists = await GetUserByUsername(user.Username);

        IList<User> users = await GetAllUsers();

        if (userExists is not null)
            return new UserWithTokenResult(ResultStatus.Fail, "User with this username already exist", null, null);

        if (users.Any(u => u.Email == user.Email))
            return new UserWithTokenResult(ResultStatus.Fail, "Email already exists.", null, null);

        int id = await _userRepository.AddUser(user);
        user.Id = id;
        await _userRepository.AddUserRoleLogin(user);

        string encryptedpassword = _encryptor.Encrypt(user.Password);

        user = await _userRepository.GetUserByUsername(user.Username);
        user.Password = encryptedpassword;

        (string generatedToken, string generatedRefreshToken) = await GenerateJwtToken(user);

        return new UserWithTokenResult(ResultStatus.Success, string.Empty, generatedToken, generatedRefreshToken);
    }

    public async Task<(int tr, int ts, double asc)> GetAdvancedInfoByUsername(string username)
    {
        User user = await _userRepository.GetUserByUsername(username);

        int totrs = await _storyRepository.TotalReadsForUser(user.Id);
        int totts = await _storyRepository.TotalStoriesForUser(user.Id);
        double toasc = await _storyRepository.AverageScoreForUser(user.Id);

        return (totrs, totts, toasc);
    }

    public async Task<LoginUserResult> LoginUser(string username, string password)
    {
        bool succeed = await _userRepository.VerifyConnection();

        if (succeed is false)
            return new LoginUserResult(ResultStatus.Fail, "Username or password are wrong", string.Empty, string.Empty);

        User user = await GetUserByUsername(username);

        if (user.Banned)
            return new LoginUserResult(ResultStatus.Fail, "This account is blocked.", string.Empty, string.Empty);

        user.Password = _encryptor.Encrypt(password);

        (string generatedToken, string generatedRefreshToken) = await GenerateJwtToken(user);

        return new LoginUserResult(ResultStatus.Success, string.Empty, generatedToken, generatedRefreshToken);
    }

    public async Task<RefreshTokenResponse> RefreshToken(string token, string refreshToken)
    {
        ClaimsPrincipal validatedToken = GetClaimsPrincipalFromToken(token);

        if (validatedToken is null)
            return new RefreshTokenResponse(ResultStatus.Fail, "Invalid token", string.Empty, string.Empty);

        long expiryDateUnix =
            long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        DateTime expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
            return new RefreshTokenResponse(ResultStatus.Fail, "The token hasn't expired yet", string.Empty,
                string.Empty);

        string jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        RefreshToken storedRefreshToken = await _userRepository.GetRefreshToken(refreshToken);

        if (storedRefreshToken is null)
            return new RefreshTokenResponse(ResultStatus.Fail, "Refresh token doesn't exist", string.Empty,
                string.Empty);

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has expired", string.Empty,
                string.Empty);

        if (storedRefreshToken.Invalidated)
            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has been invalidated", string.Empty,
                string.Empty);

        if (storedRefreshToken.Used)
            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has been used", string.Empty,
                string.Empty);

        if (storedRefreshToken.JwtId != jti)
            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has expired", string.Empty,
                string.Empty);

        storedRefreshToken.Used = true;

        await _userRepository.UpdateRefreshToken(storedRefreshToken);

        _ = int.TryParse(validatedToken.Claims.Single(x => x.Type == "id").Value, out int userId);
        User user = await _userRepository.GetUserById(userId);

        (string generatedToken, string generatedRefreshToken) = await GenerateJwtToken(user);

        return new RefreshTokenResponse(ResultStatus.Success, string.Empty, generatedToken, generatedRefreshToken);
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
        byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        string jti = Guid.NewGuid().ToString();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(JwtRegisteredClaimNames.Email, user.Username),
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("hash", user.Password)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        RefreshToken refreshToken = new(0, token.Id, jti, DateTime.UtcNow,
            DateTime.UtcNow.AddMonths(6), false, false, user.Id);

        await _userRepository.InsertRefreshToken(refreshToken);
        return (tokenHandler.WriteToken(token), refreshToken.Token);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}