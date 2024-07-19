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
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

public sealed class StoryWriteService : IStoryWriteService
{
    // TODO: add decent logging
    private readonly IStoryWriteRepository _repository;
    private readonly IIdGenerator _idGenerator;

    public StoryWriteService(IStoryWriteRepository storyWriteRepository, IIdGenerator idGenerator)
    {
        _repository = storyWriteRepository;
        _idGenerator = idGenerator;
    }

    public async Task<BaseResult> AddComment(AddCommentCommand command)
    {
        var story = await _repository.GetStory(command.StoryId);

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
        var story = await _repository.GetStory(command.StoryId);

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

    public async Task<BaseResult> CreateGenre(CreateGenreCommand command)
    {
        var genre = Genre.Create(
            _idGenerator.Generate((id) => new GenreId(id)),
            command.Name,
            command.Description,
            command.ImagePreview);

        await _repository.AddGenre(genre);

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> UpdateGenre(UpdateGenreCommand command)
    {
        var genre = await _repository.GetGenre(command.GenreId);

        if (genre is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.GenreWasNotFound);
        }

        genre.UpdateInformation(command.Name, command.Description, command.ImagePreview);

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> UpdateComment(UpdateCommentCommand command)
    {
        var story = await _repository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        story.UpdateComment(
            command.CommentId,
            command.Content,
            command.Score,
            DateTime.UtcNow);

        return BaseResult.CreateSuccess();
    }

    public async Task<UpdateStoryInfoResult> PublishStory(CreateStoryCommand command)
    {
        var story = Story.Create(
            _idGenerator.Generate((id) => new StoryId(id)),
            command.PublisherId,
            command.Title,
            command.Description,
            command.AuthorName,
            command.GenreIds.Select(id => Genre.Create(id)).ToList(),
            command.AgeLimit,
            command.ImagePreview,
            DateTime.UtcNow,
            command.DateWritten);

        await _repository.AddStory(story);

        return UpdateStoryInfoResult.CreateSuccess(story.Id);
    }

    public async Task<BaseResult> UpdateStory(UpdateStoryCommand command)
    {
        var story = await _repository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        story.UpdateInformation(
            command.Title,
            command.Description,
            command.AuthorName,
            command.GenreIds.Select(id => Genre.Create(id)).ToList(),
            command.AgeLimit,
            command.ImagePreview,
            DateTime.UtcNow,
            command.DateWritten);

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> UpdatePages(UpdateStoryPagesCommand command)
    {
        var story = await _repository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        story.SetContents(command.Contents, DateTime.UtcNow);

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> UpdateAudio(UpdateStoryAudioCommand command)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> DeleteAudio(DeleteStoryAudioCommand command)
    {
        throw new System.NotImplementedException();
    }

    public async Task<BaseResult> DeleteComment(DeleteCommentCommand command)
    {
        var story = await _repository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        story.DeleteComment(command.CommentId);

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> DeleteGenre(DeleteGenreCommand command)
    {
        var genre = await _repository.GetGenre(command.GenreId);

        if (genre is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.GenreWasNotFound);
        }

        _repository.DeleteGenre(genre);

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> DeleteStory(DeleteStoryCommand command)
    {
        var story = await _repository.GetStory(command.StoryId);

        if (story is not null)
        {
            _repository.DeleteStory(story);
        }

        return BaseResult.CreateSuccess();
    }

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
    //    //public async Task<IActionResult> AddAudioForStory([FromBody] CreateAudioModelcommand audio)
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
    //    //public async Task<IActionResult> ChangeAudioForStory([FromBody] UpdateAudiocommand audio)
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
