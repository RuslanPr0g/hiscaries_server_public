using HC.Application.DTOs;
using HC.Application.Encryption;
using HC.Application.Models.Response;
using HC.Application.Users.Command;
using HC.Application.Users.Command.LoginUser;
using HC.Application.Users.Command.PublishReview;
using HC.Application.Users.Command.RefreshToken;
using HC.Application.Users.Query;
using HC.Application.Users.Query.BecomePublisher;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC.API.Controllers;

[Route(APIConstants.User)]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class UserV1Controller : ControllerBase
{
    private const string ErrorOccuredIn = "Error occured in {0}: {1}";
    private readonly IEncryptor _encryptor;
    private readonly ILogger<UserV1Controller> _logger;
    private readonly IMediator _mediator;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public UserV1Controller(
        IMediator mediator,
        ILogger<UserV1Controller> logger,
        TokenValidationParameters tokenValidationParameters,
        IEncryptor encryptor)
    {
        _mediator = mediator;
        _logger = logger;
        _tokenValidationParameters = tokenValidationParameters;
        _encryptor = encryptor;
    }

    [HttpGet]
    public async Task<ActionResult<UserReadModel>> Get()
    {
        string username = string.Empty;

        GetUserInfoQuery query = new()
        {
            Username = username
        };

        UserReadModel model = await _mediator.Send(query);
        model.Id = _hashids.Encode(model.Id);

        return Ok(model);
    }

    [HttpGet("users")]
    public async Task<ActionResult<UserReadDto>> GetUsers()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        IList<User> users = await _userService.GetAllUsers(user);
        List<dynamic> result = new();

        foreach (User us in users)
        {
            dynamic u = us.ToDynamic();
            u.id = _hashids.Encode(us.Id);
            u.userrole = await _userService.GetUserRoleByUsername(us.Username, user);
            result.Add(u);
        }

        return Ok(result);
    }

    [HttpGet(APIConstants.BecomePublisher)]
    public async Task<ActionResult<UserReadDto>> BecomePublisher()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        BecomePublisherQuery query = new()
        {
            User = user
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpGet(APIConstants.Review)]
    public async Task<IActionResult> Reviews()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        GetReviewsQuery query = new()
        {
            User = user
        };

        List<Review> result = await _mediator.Send(query);
        List<dynamic> res = new();

        foreach (Review us in result)
        {
            dynamic u = us.ToDynamic();
            u.publisherId = _hashids.Encode(us.PublisherId);
            u.reviewerId = _hashids.Encode(us.ReviewerId);
            res.Add(u);
        }

        return Ok(res);
    }

    [HttpPost(APIConstants.Review)]
    public async Task<IActionResult> PublishReview([FromBody] PublishReviewDto publishReviewDto)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();
        int pubId = _hashids.DecodeSingle(publishReviewDto.PublisherId);

        if (pubId == id) return BadRequest("Your cannot review your profile!");

        if (user.Username is null)
            return BadRequest("Token expired");

        PublishReviewCommand query = new()
        {
            User = user,
            Id = publishReviewDto.Id,
            PublisherId = pubId,
            ReviewerId = id,
            Message = publishReviewDto.Message
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpPost(APIConstants.DeleteReview)]
    public async Task<IActionResult> DeleteReview([FromBody] PublishReviewDto publishReviewDto)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        DeleteReviewCommand query = new()
        {
            User = user,
            Id = publishReviewDto.Id
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpPatch(APIConstants.UpdateProfile)]
    public async Task<IActionResult> UpdateUserData([FromBody] UpdateUserDataUpdateDto updateUserDataDto)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        UpdateUserDataCommand query = new()
        {
            User = user,
            Email = updateUserDataDto.Email,
            Banned = updateUserDataDto.Banned,
            BirthDate = updateUserDataDto.BirthDate,
            PreviousPassword = updateUserDataDto.PreviousPassword,
            NewPassword = updateUserDataDto.NewPassword,
            UsernameUpdate = updateUserDataDto.UsernameUpdate
        };

        UpdateUserDataResult response = await _mediator.Send(query);

        if (response.ResultStatus is ResultStatus.Fail)
            return BadRequest(response.FailReason);
        return Ok(response.ResultStatus);
    }

    [AllowAnonymous]
    [HttpPost(APIConstants.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] UserCreateDto userCreateDto)
    {
        CreateUserCommand command = new()
        {
            Username = userCreateDto.Username,
            Email = userCreateDto.Email,
            BirthDate = userCreateDto.BirthDate,
            Password = userCreateDto.Password
        };

        try
        {
            RegisterUserResult result = await _mediator.Send(command);

            if (result.ResultStatus is ResultStatus.Fail)
                return BadRequest(result.FailReason);

            return Ok(new
            {
                result.Token,
                result.RefreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, nameof(UserV1Controller), ex.Message);

            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [AllowAnonymous]
    [HttpPost(APIConstants.Login)]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
    {
        LoginUserCommand command = new()
        {
            Username = userLoginDto.Username,
            Password = userLoginDto.Password
        };

        try
        {
            LoginUserResult result = await _mediator.Send(command);

            if (result.ResultStatus is ResultStatus.Fail)
                return BadRequest(result.FailReason);

            return Ok(new
            {
                result.Token,
                result.RefreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, nameof(UserV1Controller), ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPost(APIConstants.Refresh)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto userCreateDto)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        RefreshTokenCommand command = new()
        {
            User = user,
            Token = userCreateDto.Token,
            RefreshToken = userCreateDto.RefreshToken
        };

        try
        {
            RefreshTokenResponse result = await _mediator.Send(command);

            if (result.ResultStatus is ResultStatus.Fail)
                return BadRequest(result.FailReason);

            return Ok(new
            {
                result.Token,
                result.RefreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, nameof(UserV1Controller), ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    private string GetCurrentUsername()
    {
        Claim usernameClaim = null;
        if (HttpContext.User.Identity is ClaimsIdentity identity)
            usernameClaim = identity.Claims.FirstOrDefault(c => c.Type == "username");

        return usernameClaim?.Value;
    }

    private int GetCurrentId()
    {
        Claim usernameClaim = null;
        if (HttpContext.User.Identity is ClaimsIdentity identity)
            usernameClaim = identity.Claims.FirstOrDefault(c => c.Type == "id");
        bool parsed = int.TryParse(usernameClaim?.Value, out int id);
        return parsed ? id : -1;
    }

    private string GetCurrentHash()
    {
        Claim hashClaim = null;
        if (HttpContext.User.Identity is ClaimsIdentity identity)
            hashClaim = identity.Claims.FirstOrDefault(c => c.Type == "hash");

        return hashClaim?.Value is null
            ? null
            : _encryptor.Decrypt(hashClaim.Value);
    }

    public class TestClass
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}