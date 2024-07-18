using HC.API.Extensions;
using HC.API.Request;
using HC.API.Requests;
using HC.Application.Common.Extentions;
using HC.Application.Stories.Command;
using HC.Application.Stories.Command.DeleteStory;
using HC.Application.Stories.Command.ReadStory;
using HC.Application.Stories.Command.ScoreStory;
using HC.Application.Stories.Query;
using HC.Application.Stories.Query.GetGenreList;
using HC.Application.StoryPages.Command.CreateStoryPages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC.API.Controllers;

[Authorize]
[ApiController]
[Route(APIConstants.Story)]
[ApiVersion("1.0")]
public class StoryV1Controller : ControllerBase
{
    private const string ErrorOccuredIn = "Error occured in {nameof(StoryController)}: {0}";
    private readonly ILogger<StoryV1Controller> _logger;
    private readonly IMediator _mediator;

    public StoryV1Controller(
        IMediator mediator,
        ILogger<StoryV1Controller> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<StoryReadModel>>> Get([FromBody] GetStoryListRequest request)
    {
        GetStoryListQuery query = new()
        {
            Id = request.Id,
            SearchTerm = request.Search,
            Genre = request.Genre
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpPost("genres")]
    public async Task<IActionResult> AddGenre([FromBody] CreateGenreRequest request)
    {
        CreateGenreCommand command = new()
        {
            Name = request.Name,
            Description = request.Description,
            ImagePreview = request.ImagePreview
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPatch("genres")]
    public async Task<IActionResult> EditGenre([FromBody] UpdateGenreRequest request)
    {
        UpdateGenreCommand command = new()
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            ImagePreview = request.ImagePreview
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpDelete("genres")]
    public async Task<IActionResult> DeleteGenre([FromBody] DeleteGenreRequest request)
    {
        DeleteGenreCommand command = new()
        {
            Id = request.Id,
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpGet(APIConstants.Genres)]
    public async Task<ActionResult<IEnumerable<GenreReadModel>>> GetGenres()
    {
        GetGenreListQuery query = new();
        return Ok(await _mediator.Send(query));
    }

    [HttpGet(APIConstants.Shuffle)]
    public async Task<ActionResult<IEnumerable<StoryReadModel>>> BestToRead()
    {
        string username = string.Empty;

        GetStoryRecommendationsQuery query = new()
        {
            Username = username
        };

        IEnumerable<StorySimpleReadModel> result = await _mediator.Send(query);

        var response = result.ToList();
        response.Shuffle();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddStory([FromBody] StoryUpdateInfoRequest request)
    {
        string base64Image = request.ImagePreview;
        byte[] imageInBytes = new byte[10];

        if (base64Image is not null)
        {
            int offset = base64Image.IndexOf(',') + 1;
            imageInBytes = Convert.FromBase64String(base64Image[offset..]);
        }
        else
        {
            imageInBytes = null;
        }

        CreateStoryCommand command = new()
        {
            Username = string.Empty,
            Title = request.Title,
            Description = request.Description,
            AuthorName = request.AuthorName,
            GenreIds = request.GenreIds,
            AgeLimit = request.AgeLimit,
            DatePublished = DateTime.Now,
            DateWritten = request.DateWritten,
            ImagePreview = imageInBytes
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateStoryInfo([FromBody] StoryUpdateInfoRequest request)
    {
        string base64Image = request.ImagePreview;
        byte[] imageInBytes = new byte[10];

        if (base64Image is not null)
        {
            int offset = base64Image.IndexOf(',') + 1;
            imageInBytes = Convert.FromBase64String(base64Image[offset..]);
        }
        else
        {
            imageInBytes = null;
        }

        UpdateStoryCommand command = new()
        {
            Username = string.Empty,
            Title = request.Title,
            Description = request.Description,
            AuthorName = request.AuthorName,
            GenreIds = request.GenreIds,
            AgeLimit = request.AgeLimit,
            ImagePreview = imageInBytes,
            StoryId = request.StoryId.Value
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost(APIConstants.ReadStory)]
    public async Task<IActionResult> ReadStory([FromBody] ReadStoryRequest readStoryRequest)
    {
        ReadStoryCommand command = new()
        {
            UserId = new Guid(),
            StoryId = new Guid(),
            Page = readStoryRequest.PageRead
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost(APIConstants.BookmarkStory)]
    public async Task<IActionResult> BookmarkStory([FromBody] BookmarkStoryRequest readStoryRequest)
    {
        BookmarkStoryCommand command = new()
        {
            UserId = new Guid(),
            StoryId = new Guid()
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost(APIConstants.Pages)]
    public async Task<IActionResult> UpdatePages([FromBody] StoryPagesCreateRequest request)
    {
        UpdateStoryPagesCommand command = new()
        {
            StoryId = request.StoryId,
            Content = request.Content
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }


    [HttpPost(APIConstants.AddComment)]
    public async Task<IActionResult> AddComment([FromBody] CreateCommentRequest request)
    {
        AddCommentCommand command = new()
        {
            StoryId = request.StoryId,
            UserId = new Guid(),
            Content = request.Content,
            CommentedAt = DateTime.Now
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost(APIConstants.Score)]
    public async Task<IActionResult> ScoreStory([FromBody] ScoreStoryRequest request)
    {
        StoryScoreCommand command = new()
        {
            StoryId = request.StoryId,
            Score = request.Score,
            UserId = new Guid()
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost(APIConstants.DeleteComment)]
    public async Task<IActionResult> DeleteComment([FromQuery] Guid commentId)
    {
        DeleteCommentCommand command = new()
        {
            Id = commentId
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost(APIConstants.DeleteStory)]
    public async Task<IActionResult> DeleteStory([FromQuery] Guid storyId)
    {
        DeleteStoryCommand command = new()
        {
            StoryId = storyId
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPatch(APIConstants.UpdateComment)]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest request)
    {
        UpdateCommentCommand command = new()
        {
            UserId = new Guid(),
            Id = request.Id,
            StoryId = request.StoryId,
            Content = request.Content
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPost("audio")]
    [AllowAnonymous]
    public async Task<IActionResult> AddAudioForStory([FromBody] UpdateAudioRequest request)
    {
        UpdateStoryAudioCommand command = new()
        {
            UserId = new Guid(),
            StoryId = request.StoryId,
            Name = request.Name,
            Audio = request.Audio
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpPut("audio")]
    public async Task<IActionResult> ChangeAudioForStory([FromBody] UpdateAudioRequest request)
    {
        UpdateStoryAudioCommand command = new()
        {
            UserId = new Guid(),
            StoryId = request.StoryId,
            Name = request.Name,
            Audio = request.Audio
        };

        return (await _mediator.Send(command)).ToObjectResult();
    }

    [HttpDelete("audio")]
    public async Task<IActionResult> DeleteAudioForStory([FromBody] Guid storyId)
    {
        DeleteStoryAudioCommand command = new()
        {
            StoryId = storyId
        };

        return (await _mediator.Send(command)).ToObjectResult();
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