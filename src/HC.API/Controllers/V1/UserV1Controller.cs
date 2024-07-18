using HC.API.Extensions;
using HC.API.Requests;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC.API.Controllers;

[Authorize]
[Route(APIConstants.User)]
[ApiController]
[ApiVersion("1.0")]
public class UserV1Controller : ControllerBase
{
    private const string ErrorOccuredIn = "Error occured in {0}: {1}";
    private readonly ILogger<UserV1Controller> _logger;
    private readonly IMediator _mediator;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public UserV1Controller(
        IMediator mediator,
        ILogger<UserV1Controller> logger,
        TokenValidationParameters tokenValidationParameters)
    {
        _mediator = mediator;
        _logger = logger;
        _tokenValidationParameters = tokenValidationParameters;
    }

    [HttpGet]
    public async Task<ActionResult<UserReadModel>> Get()
    {
        // TODO: add jwt auth
        // https://www.youtube.com/watch?v=mgeuh8k3I4g&t=550s&ab_channel=NickChapsas

        string username = string.Empty;

        GetUserInfoQuery query = new()
        {
            Username = username
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("info")]
    public async Task<ActionResult<UserReadModel>> Get([FromQuery] string username)
    {
        GetUserInfoQuery query = new()
        {
            Username = username
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpGet(APIConstants.BecomePublisher)]
    public async Task<IActionResult> BecomePublisher()
    {
        BecomePublisherCommand command = new()
        {
            Username = string.Empty
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost(APIConstants.Review)]
    public async Task<IActionResult> PublishReview([FromBody] PublishReviewRequest request)
    {
        PublishReviewCommand query = new()
        {
            Id = request.Id,
            PublisherId = request.PublisherId,
            ReviewerId = request.ReviewerId,
            Message = request.Message
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpPost(APIConstants.DeleteReview)]
    public async Task<IActionResult> DeleteReview([FromBody] DeleteReviewRequest request)
    {
        DeleteReviewCommand query = new()
        {
            Username = string.Empty,
            Id = request.Id
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpPatch(APIConstants.UpdateProfile)]
    public async Task<IActionResult> UpdateUserData([FromBody] UpdateUserDataRequest request)
    {
        UpdateUserDataCommand query = new()
        {
            Username = string.Empty,
            Email = request.Email,
            Banned = request.Banned,
            BirthDate = request.BirthDate,
            PreviousPassword = request.PreviousPassword,
            NewPassword = request.NewPassword,
            UpdatedUsername = request.UpdatedUsername
        };

        return Ok(await _mediator.Send(query));
    }

    [AllowAnonymous]
    [HttpPost(APIConstants.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        RegisterUserCommand command = new()
        {
            Username = request.Username,
            Email = request.Email,
            BirthDate = request.BirthDate,
            Password = request.Password
        };

        UserWithTokenResult result = await _mediator.Send(command);

        return Ok(new
        {
            result.Token,
            result.RefreshToken
        });
    }

    [AllowAnonymous]
    [HttpPost(APIConstants.Login)]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginRequest request)
    {
        LoginUserCommand command = new()
        {
            Username = request.Username,
            Password = request.Password
        };

        LoginUserResult result = await _mediator.Send(command);

        return Ok(new
        {
            result.Token,
            result.RefreshToken
        });
    }

    [HttpPost(APIConstants.Refresh)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        RefreshTokenCommand command = new()
        {
            Token = request.Token,
            RefreshToken = request.RefreshToken
        };

        RefreshTokenResponse result = await _mediator.Send(command);

        return Ok(new
        {
            result.Token,
            result.RefreshToken
        });
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
}