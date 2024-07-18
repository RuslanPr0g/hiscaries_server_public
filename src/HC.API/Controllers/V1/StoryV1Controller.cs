using HashidsNet;
using HC.API.RequestQuery;
using HC.Application.Common.Extentions;
using HC.API.Requests;
using HC.Application.Encryption;
using HC.Application.Models.Response;
using HC.Application.Stories.Command;
using HC.Application.Stories.Command.ReadStory;
using HC.Application.Stories.Command.ScoreStory;
using HC.Application.Stories.DeleteStory;
using HC.Application.Stories.Query;
using HC.Application.Stories.Query.Bookmarks;
using HC.Application.Stories.Query.GetGenreList;
using HC.Application.Stories.Query.History;
using HC.Application.StoryPages.Command;
using HC.Application.StoryPages.Command.CreateStoryPages;
using HC.Application.StoryPages.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC.API.Controllers;

[ApiController]
[Route(APIConstants.Story)]
[ApiVersion("1.0")]
[Authorize]
public class StoryV1Controller : ControllerBase
{
    private const string ErrorOccuredIn = "Error occured in {nameof(StoryController)}: {0}";
    private readonly IEncryptor _encryptor;
    private readonly IHashids _hashids;
    private readonly ILogger<StoryV1Controller> _logger;
    private readonly IMediator _mediator;

    public StoryV1Controller(
        IMediator mediator,
        ILogger<StoryV1Controller> logger,
        IEncryptor encryptor,
        IHashids hashids)
    {
        _mediator = mediator;
        _logger = logger;
        _encryptor = encryptor;
        _storyRepository = storyRepository;
        _hashids = hashids;
    }

    [HttpGet]
    public async Task<ActionResult<List<Story>>> Get([FromQuery] GetStoryListRequestQuery getStoryListRequestQuery)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        int storyId = string.IsNullOrEmpty(getStoryListRequestQuery.Id)
            ? 0
            : _hashids.DecodeSingle(getStoryListRequestQuery.Id);

        GetStoryListQuery query = new()
        {
            User = user,
            Id = storyId,
            Search = getStoryListRequestQuery.Search,
            Genre = getStoryListRequestQuery.Genre
        };

        List<StoryReadRequest> result = await _mediator.Send(query);
        List<dynamic> res = new();

        foreach (StoryReadRequest story in result)
        {
            dynamic st = story.ToDynamic();
            st.id = _hashids.Encode(story.Id);
            st.publisherId = _hashids.Encode(story.Publisher.Id);
            res.Add(st);
        }

        return Ok(res);
    }

    [HttpPost("genres")]
    public async Task<IActionResult> AddGenre([FromBody] Genre genre)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        return Ok(await _storyRepository.AddGenre(genre, user));
    }

    [HttpPatch("genre")]
    public async Task<IActionResult> EditGenre([FromBody] Genre genre)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        return Ok(await _storyRepository.UpdateGenre(genre, user));
    }

    [HttpPost("genre/delete")]
    public async Task<IActionResult> DeleteGenre([FromBody] Genre genre)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        return Ok(await _storyRepository.DeleteGenre(genre, user));
    }

    [HttpGet("stories")]
    public async Task<ActionResult<List<Story>>> Get()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        GetStoryListQuery query = new()
        {
            User = user,
            All = true
        };

        List<StoryReadRequest> stories = await _mediator.Send(query);
        List<dynamic> result = new();

        foreach (StoryReadRequest story in stories)
        {
            dynamic st = story.ToDynamic();
            st.id = _hashids.Encode(story.Id);
            st.publisherId = _hashids.Encode(story.Publisher.Id);
            result.Add(st);
        }

        return Ok(result);
    }

    [HttpGet(APIConstants.Genres)]
    public async Task<ActionResult<List<Story>>> GetGenres()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        GetGenreListQuery query = new()
        {
            User = user
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpGet(APIConstants.Shuffle)]
    public async Task<ActionResult<List<Story>>> BestToRead()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        GetStoryListQuery query = new()
        {
            User = user
        };

        List<StoryReadRequest> result = await _mediator.Send(query);
        List<dynamic> res = new();

        foreach (StoryReadRequest story in result)
        {
            dynamic st = story.ToDynamic();
            st.id = _hashids.Encode(story.Id);
            st.publisherId = _hashids.Encode(story.Publisher.Id);
            res.Add(st);
        }

        //var notEmptyStories = res.Where(s => s.PageCount > 0);
        List<dynamic> notEmptyStories = res;

        List<dynamic> listedStories = notEmptyStories.ToList();

        listedStories.Shuffle();

        return Ok(listedStories);
    }

    [HttpGet(APIConstants.GetPage)]
    public async Task<ActionResult<List<StoryPage>>> GetPages(string storyId)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        GetStoryPageListQuery query = new()
        {
            User = user,
            StoryId = _hashids.DecodeSingle(storyId)
        };

        return Ok(await _mediator.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> AddStory([FromBody] StoryCreateRequest storyCreateRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        string base64Image = storyCreateRequest.ImagePreview;
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
            User = user,
            Title = storyCreateRequest.Title,
            Description = storyCreateRequest.Description,
            AuthorName = storyCreateRequest.AuthorName,
            GenreIds = storyCreateRequest.GenreIds,
            AgeLimit = storyCreateRequest.AgeLimit,
            DatePublished = DateTime.Now,
            DateWritten = storyCreateRequest.DateWritten,
            ImagePreview = imageInBytes
        };

        try
        {
            PublishStoryResult result = await _mediator.Send(command);

            if (result.ResultStatus is ResultStatus.Fail)
                return BadRequest(result.FailReason);

            return Ok(new { id = _hashids.Encode(result.Id) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateStoryInfo([FromBody] StoryCreateRequest storyCreateRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        string base64Image = storyCreateRequest.ImagePreview;
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
            User = user,
            Title = storyCreateRequest.Title,
            Description = storyCreateRequest.Description,
            AuthorName = storyCreateRequest.AuthorName,
            GenreIds = storyCreateRequest.GenreIds,
            AgeLimit = storyCreateRequest.AgeLimit,
            ImagePreview = imageInBytes,
            StoryId = _hashids.DecodeSingle(storyCreateRequest.StoryId)
        };

        try
        {
            PublishStoryResult result = await _mediator.Send(command);

            if (result.ResultStatus is ResultStatus.Fail)
                return BadRequest(result.FailReason);

            return Ok(new { id = _hashids.Encode(result.Id) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpGet(APIConstants.History)]
    public async Task<IActionResult> History()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null)
            return BadRequest("Token expired");

        HistoryQuery query = new()
        {
            User = user,
            UserId = id
        };

        try
        {
            List<StoryReadHistoryProgress> result = await _mediator.Send(query);
            List<dynamic> res = new();

            foreach (StoryReadHistoryProgress story in result)
            {
                dynamic st = story.ToDynamic();
                st.storyId = _hashids.Encode(story.StoryId);
                st.publisherId = _hashids.Encode(story.PublisherId);
                res.Add(st);
            }

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPost(APIConstants.ReadStory)]
    public async Task<IActionResult> ReadStory([FromBody] ReadStoryRequest readStoryRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();
        int storyId = _hashids.DecodeSingle(readStoryRequest.StoryId);

        if (user.Username is null || id < 0 || storyId < 0)
            return BadRequest("Token expired");

        ReadStoryCommand command = new()
        {
            User = user,
            UserId = id,
            StoryId = storyId,
            Page = readStoryRequest.PageRead
        };

        try
        {
            int result = await _mediator.Send(command);

            return Ok(new { id = _hashids.Encode(result) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpGet(APIConstants.BookmarkStory)]
    public async Task<IActionResult> GetBookmarksStory()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null || id < 0)
            return BadRequest("Token expired");

        BookmarkStoryQuery command = new()
        {
            User = user,
            UserId = id
        };

        try
        {
            List<Story> result = await _mediator.Send(command);
            List<dynamic> res = new();

            foreach (Story story in result)
            {
                dynamic st = story.ToDynamic();
                st.id = _hashids.Encode(story.Id);
                st.publisherId = _hashids.Encode(story.PublisherId);
                res.Add(st);
            }

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPost(APIConstants.BookmarkStory)]
    public async Task<IActionResult> BookmarkStory([FromBody] BookmarkStoryRequest readStoryRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();
        int storyId = _hashids.DecodeSingle(readStoryRequest.StoryId);

        if (user.Username is null || id < 0 || storyId < 0)
            return BadRequest("Token expired");

        BookmarkStoryCommand command = new()
        {
            User = user,
            UserId = id,
            StoryId = storyId
        };

        try
        {
            PublishStoryResult result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPost(APIConstants.AddPage)]
    public async Task<IActionResult> AddPage([FromBody] StoryPageCreateRequest storyPageCreateRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        CreateStoryPageCommand command = new()
        {
            User = user,
            StoryId = _hashids.DecodeSingle(storyPageCreateRequest.StoryId),
            Content = storyPageCreateRequest.Content,
            Page = storyPageCreateRequest.Page
        };

        try
        {
            AddStoryPageResult result = await _mediator.Send(command);

            if (result.ResultStatus is ResultStatus.Fail)
                return BadRequest(result.FailReason);

            return Ok("Story page added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPost(APIConstants.UpdatePages)]
    public async Task<IActionResult> UpdatePages([FromBody] StoryPagesCreateRequest storyPageCreateRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        CreateStoryPagesCommand command = new()
        {
            User = user,
            StoryId = _hashids.DecodeSingle(storyPageCreateRequest.StoryId),
            Content = storyPageCreateRequest.Content
        };

        try
        {
            AddStoryPageResult result = await _mediator.Send(command);

            if (result.ResultStatus is ResultStatus.Fail)
                return BadRequest(result.FailReason);

            return Ok("Story pages added successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }


    [HttpPost(APIConstants.AddComment)]
    public async Task<IActionResult> AddComment([FromBody] CreateCommentRequest createCommentRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int userId = GetCurrentId();

        if (user.Username is null || userId < 0)
            return BadRequest("Token expired");

        AddCommentCommand command = new()
        {
            User = user,
            Comment = new Comment
            {
                StoryId = _hashids.DecodeSingle(createCommentRequest.StoryId),
                UserId = userId,
                Content = createCommentRequest.Content,
                CommentedAt = DateTime.Now
            }
        };

        try
        {
            int result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500,
                "You didn't read the story to score or comment it! Or you an author ;) you cannot do this with your story!");
        }
    }

    [HttpGet(APIConstants.Score)]
    public async Task<IActionResult> GetScoreStory()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null || id < 0)
            return BadRequest("Token expired");

        StoryScoreQuery command = new()
        {
            User = user
        };

        try
        {
            List<StoryRating> result = await _mediator.Send(command);
            List<dynamic> res = new();

            foreach (StoryRating story in result)
            {
                dynamic st = story.ToDynamic();
                st.storyId = _hashids.Encode(story.StoryId);
                st.userId = _hashids.Encode(story.UserId);
                res.Add(st);
            }

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPost(APIConstants.Score)]
    public async Task<IActionResult> ScoreStory([FromBody] ScoreStoryRequest createCommentRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null || id < 0)
            return BadRequest("Token expired");

        StoryScoreCommand command = new()
        {
            User = user,
            StoryId = _hashids.DecodeSingle(createCommentRequest.StoryId),
            Score = createCommentRequest.Score,
            UserId = id
        };

        try
        {
            int result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500,
                "You didn't read the story to score or comment it! Or you an author ;) you cannot do this with your story!");
        }
    }

    [HttpPost(APIConstants.DeleteComment)]
    public async Task<IActionResult> DeleteComment([FromBody] CreateCommentRequest createCommentRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null || id < 0)
            return BadRequest("Token expired");

        DeleteCommentCommand command = new()
        {
            User = user,
            Id = createCommentRequest.Id
        };

        try
        {
            int result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPost(APIConstants.DeleteStory)]
    public async Task<IActionResult> DeleteComment([FromBody] ScoreStoryRequest createCommentRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null || id < 0)
            return BadRequest("Token expired");

        DeleteStoryCommand command = new()
        {
            User = user,
            StoryId = _hashids.DecodeSingle(createCommentRequest.StoryId)
        };

        try
        {
            int result = await _mediator.Send(command);

            return Ok(new { id = _hashids.Encode(result) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpPatch(APIConstants.UpdateComment)]
    public async Task<IActionResult> UpdateComment([FromBody] CreateCommentRequest createCommentRequest)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null || id < 0)
            return BadRequest("Token expired");

        UpdateCommentCommand command = new()
        {
            User = user,
            UserId = id,
            Id = createCommentRequest.Id,
            StoryId = _hashids.DecodeSingle(createCommentRequest.StoryId),
            Content = createCommentRequest.Content
        };

        try
        {
            int result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpGet(APIConstants.GetAllComments)]
    public async Task<IActionResult> GetAllComments()
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        int id = GetCurrentId();

        if (user.Username is null || id < 0)
            return BadRequest("Token expired");

        GetAllCommentsQuery command = new()
        {
            User = user
        };

        try
        {
            List<Comment> result = await _mediator.Send(command);
            List<dynamic> res = new();

            foreach (Comment story in result)
            {
                dynamic st = story.ToDynamic();
                st.storyId = _hashids.Encode(story.StoryId);
                st.userId = _hashids.Encode(story.UserId);
                res.Add(st);
            }

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorOccuredIn, ex.Message);
            return StatusCode(500, "Our monkey team is working on a problem right now");
        }
    }

    [HttpGet("audio")]
    public async Task<IActionResult> GetAudioForStory([FromQuery] int storyId)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        List<StoryAudio> audioModels = await _storyRepository.GetAudio(storyId, user);
        StoryAudio story = audioModels.FirstOrDefault();

        byte[] result = await System.IO.File.ReadAllBytesAsync("audios/" + story?.FileId + ".mp3");

        List<GetStoryFilesRequest> storyFilesRead = audioModels.Select(x =>
            new GetStoryFiles(x.Id, x.FileId, x.DateAdded, x.Name, result)).ToList();

        return Ok(storyFilesRead);
    }

    [HttpPost("audio")]
    [AllowAnonymous]
    public async Task<IActionResult> AddAudioForStory([FromBody] CreateAudioModelRequest audio)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        Guid newAudioId = Guid.NewGuid();
        DateTimeOffset currentDate = DateTimeOffset.Now;
        StoryAudio storyAudio = new(newAudioId, currentDate.Date, audio.Name);

        SaveByteArrayToFileWithBinaryWriter(audio.Audio, "audios/" + newAudioId + ".mp3");

        int result = await _storyRepository.CreateAudio(storyAudio, user);
        return Ok(result);
    }

    public static void SaveByteArrayToFileWithBinaryWriter(byte[] data, string filePath)
    {
        using BinaryWriter writer = new(System.IO.File.OpenWrite(filePath));
        writer.Write(data);
    }

    [HttpPut("audio")]
    public async Task<IActionResult> ChangeAudioForStory([FromBody] UpdateAudioRequest audio)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        StoryAudio existingAudio = await _storyRepository.GetAudioById(audio.AudioId);

        if (existingAudio is null)
            return NotFound("Audio not found");

        SaveByteArrayToFileWithBinaryWriter(audio.Audio, "audios/" + existingAudio.FileId + ".mp3");

        return Ok();
    }

    [HttpDelete("audio")]
    public async Task<IActionResult> DeleteAudioForStory([FromBody] int[] audioIds)
    {
        UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

        if (user.Username is null)
            return BadRequest("Token expired");

        StoryAudio existingAudio = await _storyRepository.GetAudioById(audioIds[0]);

        if (existingAudio is null)
            return NotFound("Audio not found");

        bool result = await _storyRepository.DeleteAudio(audioIds, user);

        string path = "audios/" + existingAudio.FileId + ".mp3";

        if (!result || !System.IO.File.Exists(path)) return NoContent();

        System.IO.File.Delete(path);
        return Ok();
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
}