using HC.Application.Models.Response;
using HC.Application.Stories.Command;
using HC.Application.Stories.Command.ReadStory;
using HC.Application.Stories.Command.ScoreStory;
using HC.Application.Stories.DeleteStory;
using HC.Domain.Stories;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IStoryWriteService
{
    Task<Story> GetStoryById(StoryId storyId);
    Task<PublishStoryResult> PublishStory(CreateStoryCommand command);
    Task<BaseResult> DeleteStory(DeleteStoryCommand command);
    Task<BaseResult> DeleteComment(DeleteCommentCommand command);
    Task<PublishStoryResult> UpdateStory(UpdateStoryCommand command);
    Task<BaseResult> AddComment(AddCommentCommand command);
    Task<BaseResult> AddStoryScore(StoryScoreCommand command);
    Task<PublishStoryResult> BookmarkStory(BookmarkStoryCommand command);
    Task<BaseResult> AddImageToStory(StoryId storyId, string imagePath);
    Task<BaseResult> AddImageToStoryByBase64(StoryId storyId, byte[] base64);
    Task<BaseResult> ReadStoryHistory(ReadStoryCommand command);
}