using HC.Application.Common.Constants;
using HC.Application.Interface;
using HC.Application.Interface.Generators;
using HC.Application.Models.Response;
using HC.Application.Stories.Command;
using HC.Application.Stories.Command.DeleteStory;
using HC.Application.Stories.Command.ReadStory;
using HC.Application.Stories.Command.ScoreStory;
using HC.Application.StoryPages.Command.CreateStoryPages;
using HC.Domain.Stories;
using System;
using System.Threading.Tasks;

public sealed class StoryWriteService : IStoryWriteService
{
    // TODO: add decent logging
    private readonly IStoryWriteRepository _storyWriteRepository;
    private readonly IIdGenerator _idGenerator;

    public StoryWriteService(IStoryWriteRepository storyWriteRepository, IIdGenerator idGenerator)
    {
        _storyWriteRepository = storyWriteRepository;
        _idGenerator = idGenerator;
    }

    public async Task<BaseResult> AddComment(AddCommentCommand command)
    {
        var story = await _storyWriteRepository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        story.AddComment(
            _idGenerator.Generate((id) => new CommentId(id)),
            command.UserId,
            command.Content,
            command.Score,
            DateTime.UtcNow);

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> SetStoryScoreForAUser(StoryScoreCommand command)
    {
        var story = await _storyWriteRepository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        story.SetScoreByUser(
            command.UserId,
            command.Score,
            _idGenerator.Generate((id) => new StoryRatingId(id)));

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> CreateGenre(CreateGenreCommand request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<UpdateStoryInfoResult> PublishStory(CreateStoryCommand command)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> UpdateAudio(UpdateStoryAudioCommand request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> UpdateComment(UpdateCommentCommand request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> UpdateGenre(UpdateGenreCommand request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<AddStoryPageResult> UpdatePages(UpdateStoryPagesCommand request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> UpdateStory(UpdateStoryCommand command)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> DeleteAudio(DeleteStoryAudioCommand request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> DeleteComment(DeleteCommentCommand command)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> DeleteGenre(DeleteGenreCommand request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> DeleteStory(DeleteStoryCommand command)
    {
        throw new System.NotImplementedException();
    }

    //    //[HttpGet("audio")]
    //    //public async Task<IActionResult> GetAudioForStory([FromQuery] int storyId)
    //    //{
    //    //    UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

    //    //    if (user.Username is null)
    //    //        return BadRequest("Token expired");

    //    //    List<StoryAudio> audioModels = await _storyRepository.GetAudio(storyId, user);
    //    //    StoryAudio story = audioModels.FirstOrDefault();

    //    //    byte[] result = await System.IO.File.ReadAllBytesAsync("audios/" + story?.FileId + ".mp3");

    //    //    List<GetStoryFilesRequest> storyFilesRead = audioModels.Select(x =>
    //    //        new GetStoryFiles(x.Id, x.FileId, x.DateAdded, x.Name, result)).ToList();

    //    //    return Ok(storyFilesRead);
    //    //}

    //    public async Task AddImageToStory(int storyId, string imagePath)
    //    {
    //        Story story = await _storyRepository.GetStory(storyId);
    //        story.SetImage(ImageHelper.ImageToByteArrayFromFilePath(imagePath));
    //        await _storyRepository.UpdateStory(story);
    //    }

    //    public async Task AddImageToStoryByBase64(int storyId, byte[] base64)
    //    {
    //        Story story = await _storyRepository.GetStory(storyId);
    //        story.SetImage(base64);
    //        await _storyRepository.UpdateStory(story);
    //    }

    //    //[HttpPost("audio")]
    //    //[AllowAnonymous]
    //    //public async Task<IActionResult> AddAudioForStory([FromBody] CreateAudioModelRequest audio)
    //    //{
    //    //    Guid newAudioId = Guid.NewGuid();
    //    //    DateTimeOffset currentDate = DateTimeOffset.Now;
    //    //    StoryAudio storyAudio = new(newAudioId, currentDate.Date, audio.Name);

    //    //    SaveByteArrayToFileWithBinaryWriter(audio.Audio, "audios/" + newAudioId + ".mp3");

    //    //    int result = await _storyRepository.CreateAudio(storyAudio, user);
    //    //    return Ok(result);
    //    //}

    //    //public static void SaveByteArrayToFileWithBinaryWriter(byte[] data, string filePath)
    //    //{
    //    //    using BinaryWriter writer = new(System.IO.File.OpenWrite(filePath));
    //    //    writer.Write(data);
    //    //}

    //    //[HttpPut("audio")]
    //    //public async Task<IActionResult> ChangeAudioForStory([FromBody] UpdateAudioRequest audio)
    //    //{
    //    //    StoryAudio existingAudio = await _storyRepository.GetAudioById(audio.AudioId);

    //    //    if (existingAudio is null)
    //    //        return NotFound("Audio not found");

    //    //    SaveByteArrayToFileWithBinaryWriter(audio.Audio, "audios/" + existingAudio.FileId + ".mp3");

    //    //    return Ok();
    //    //}

    //    //[HttpDelete("audio")]
    //    //public async Task<IActionResult> DeleteAudioForStory([FromBody] int[] audioIds)
    //    //{
    //    //    StoryAudio existingAudio = await _storyRepository.GetAudioById(audioIds[0]);

    //    //    if (existingAudio is null)
    //    //        return NotFound("Audio not found");

    //    //    bool result = await _storyRepository.DeleteAudio(audioIds, user);

    //    //    string path = "audios/" + existingAudio.FileId + ".mp3";

    //    //    if (!result || !System.IO.File.Exists(path)) return NoContent();

    //    //    System.IO.File.Delete(path);
    //    //    return Ok();
    //    //}
    //}
}
