// public class UserService : IUserWriteService
// {
//    private readonly DbConnectionRoleCreator _dbConnectionRoleCreator;
//    private readonly IEncryptor _encryptor;
//    private readonly JwtSettings _jwtSettings;
//    private readonly IStoryWriteRepository _storyRepository;
//    private readonly TokenValidationParameters _tokenValidationParameters;
//    private readonly IUserWriteRepository _userRepository;

//    public UserService(IUserWriteRepository userRepository, JwtSettings jwtSettings,
//        TokenValidationParameters tokenValidationParameters,
//        IOptionsSnapshot<DbConnectionRoleCreator> connection, IEncryptor encryptor,
//        IStoryWriteRepository storyRepository)
//    {
//        _userRepository = userRepository;
//        _jwtSettings = jwtSettings;
//        _tokenValidationParameters = tokenValidationParameters;
//        _dbConnectionRoleCreator = connection.Value;
//        _encryptor = encryptor;
//        _storyRepository = storyRepository;
//    }

// User userExists = await _userService.GetUserByUsername(request.Username);

// if (!string.IsNullOrEmpty(request.UpdatedUsername))
//    userExists = await _userService.GetUserByUsername(request.UpdatedUsername);

// IList<User> users = await _userService.GetAllUsers();
// UpdateUserDataResult updateUserDataResult = new(ResultStatus.Success, string.Empty);
// if (userExists is null)
//    return new UpdateUserDataResult(ResultStatus.Fail, "Something went wrong.");

// if (request.Email != userExists.Email && users.Any(u => u.Email == request.Email) && request.Banned == false)
//    return new UpdateUserDataResult(ResultStatus.Fail, "Email already exists.");

// if (!string.IsNullOrEmpty(request.NewPassword))
// {
//    // TODO:
//    // updateUserDataResult = await _userService.UpdateUserPassword(request.Username, request.PreviousPassword, request.NewPassword);
// }
// _ = await _userService.UpdateUserData(
// new User(0,
// string.IsNullOrEmpty(request.UpdatedUsername) ? request.Username : request.UpdatedUsername,
// request.Email, )
// {
//    Username = ,
//    Email = ,
//        BirthDate = request.BirthDate,
//        Banned = request.Banned
//    }, request.User);

// return updateUserDataResult;

//    public async Task<UserWithTokenResult> RegisterUser(User user)
//    {
//        User userExists = await GetUserByUsername(user.Username);

//        IList<User> users = await GetAllUsers();

//        if (userExists is not null)
//            return new UserWithTokenResult(ResultStatus.Fail, "User with this username already exist", null, null);

//        if (users.Any(u => u.Email == user.Email))
//            return new UserWithTokenResult(ResultStatus.Fail, "Email already exists.", null, null);

//        int id = await _userRepository.AddUser(user);
//        user.Id = id;
//        await _userRepository.AddUserRoleLogin(user);

//        string encryptedpassword = _encryptor.Encrypt(user.Password);

//        user = await _userRepository.GetUserByUsername(user.Username);
//        user.Password = encryptedpassword;

//        (string generatedToken, string generatedRefreshToken) = await GenerateJwtToken(user);

//        return new UserWithTokenResult(ResultStatus.Success, string.Empty, generatedToken, generatedRefreshToken);
//    }

//    public async Task<LoginUserResult> LoginUser(string username, string password)
//    {
//        bool succeed = await _userRepository.VerifyConnection();

//        if (succeed is false)
//            return new LoginUserResult(ResultStatus.Fail, "Username or password are wrong", string.Empty, string.Empty);

//        User user = await GetUserByUsername(username);

//        if (user.Banned)
//            return new LoginUserResult(ResultStatus.Fail, "This account is blocked.", string.Empty, string.Empty);

//        user.Password = _encryptor.Encrypt(password);

//        (string generatedToken, string generatedRefreshToken) = await GenerateJwtToken(user);

//        return new LoginUserResult(ResultStatus.Success, string.Empty, generatedToken, generatedRefreshToken);
//    }

//    public async Task<RefreshTokenResponse> RefreshToken(string token, string refreshToken)
//    {
//        ClaimsPrincipal validatedToken = GetClaimsPrincipalFromToken(token);

//        if (validatedToken is null)
//            return new RefreshTokenResponse(ResultStatus.Fail, "Invalid token", string.Empty, string.Empty);

//        long expiryDateUnix =
//            long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

//        DateTime expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            .AddSeconds(expiryDateUnix);

//        if (expiryDateTimeUtc > DateTime.UtcNow)
//            return new RefreshTokenResponse(ResultStatus.Fail, "The token hasn't expired yet", string.Empty,
//                string.Empty);

//        string jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

//        RefreshToken storedRefreshToken = await _userRepository.GetRefreshToken(refreshToken);

//        if (storedRefreshToken is null)
//            return new RefreshTokenResponse(ResultStatus.Fail, "Refresh token doesn't exist", string.Empty,
//                string.Empty);

//        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
//            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has expired", string.Empty,
//                string.Empty);

//        if (storedRefreshToken.Invalidated)
//            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has been invalidated", string.Empty,
//                string.Empty);

//        if (storedRefreshToken.Used)
//            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has been used", string.Empty,
//                string.Empty);

//        if (storedRefreshToken.JwtId != jti)
//            return new RefreshTokenResponse(ResultStatus.Fail, "The refresh token has expired", string.Empty,
//                string.Empty);

//        storedRefreshToken.Used = true;

//        await _userRepository.UpdateRefreshToken(storedRefreshToken);

//        _ = int.TryParse(validatedToken.Claims.Single(x => x.Type == "id").Value, out int userId);
//        User user = await _userRepository.GetUserById(userId);

//        (string generatedToken, string generatedRefreshToken) = await GenerateJwtToken(user);

//        return new RefreshTokenResponse(ResultStatus.Success, string.Empty, generatedToken, generatedRefreshToken);
//    }
// }