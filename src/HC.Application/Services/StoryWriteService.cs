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
using HC.Domain.Users;
using MediatR;
using System;
using System.IO;
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
        var story = await _repository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        Guid fileId = Guid.NewGuid();

        story.SetAudio(
            _idGenerator.Generate((id) => new StoryAudioId(id)),
            fileId,
            DateTime.UtcNow,
            command.Name);

        SaveByteArrayToFileWithBinaryWriter(command.Audio, "audios/" + fileId + ".mp3");

        return BaseResult.CreateSuccess();
    }

    public async Task<BaseResult> DeleteAudio(DeleteStoryAudioCommand command)
    {
        var story = await _repository.GetStory(command.StoryId);

        if (story is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.StoryWasNotFound);
        }

        Guid? removedAudioId = story.ClearAllAudio();

        if (removedAudioId.HasValue)
        {
            string path = "audios/" + removedAudioId + ".mp3";

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        return BaseResult.CreateSuccess();
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

    private static void SaveByteArrayToFileWithBinaryWriter(byte[] data, string filePath)
    {
        using BinaryWriter writer = new(File.OpenWrite(filePath));
        writer.Write(data);
    }
}
