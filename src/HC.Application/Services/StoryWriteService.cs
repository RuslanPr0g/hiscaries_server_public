using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Application.Stories.Command;
using HC.Application.Stories.Command.DeleteStory;
using HC.Application.Stories.Command.ReadStory;
using HC.Application.Stories.Command.ScoreStory;
using HC.Application.StoryPages.Command.CreateStoryPages;
using HC.Domain.Stories;
using System.Threading.Tasks;

public sealed class StoryWriteService : IStoryWriteService
{
    public Task<BaseResult> AddComment(AddCommentCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> AddImageToStory(StoryId storyId, string imagePath)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> AddImageToStoryByBase64(StoryId storyId, byte[] base64)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> AddStoryScore(StoryScoreCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<UpdateStoryInfoResult> BookmarkStory(BookmarkStoryCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> CreateGenre(CreateGenreCommand request)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> DeleteAudio(DeleteStoryAudioCommand request)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> DeleteComment(DeleteCommentCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> DeleteGenre(DeleteGenreCommand request)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> DeleteStory(DeleteStoryCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<Story> GetStoryById(StoryId storyId)
    {
        throw new System.NotImplementedException();
    }

    public Task<UpdateStoryInfoResult> PublishStory(CreateStoryCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> ReadStoryHistory(ReadStoryCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> UpdateAudio(UpdateStoryAudioCommand request)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> UpdateComment(UpdateCommentCommand request)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> UpdateGenre(UpdateGenreCommand request)
    {
        throw new System.NotImplementedException();
    }

    public Task<AddStoryPageResult> UpdatePages(UpdateStoryPagesCommand request)
    {
        throw new System.NotImplementedException();
    }

    public Task<UpdateStoryInfoResult> UpdateStory(UpdateStoryCommand command)
    {
        throw new System.NotImplementedException();
    }
}
