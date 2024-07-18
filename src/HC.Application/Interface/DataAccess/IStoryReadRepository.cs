using HC.Domain.Stories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IStoryReadRepository
{
    Task<IEnumerable<StorySimpleReadModel>> GetStories();
    Task<IEnumerable<StorySimpleReadModel>> GetStoriesBy(string searchTerm, string genre);
    Task<IEnumerable<StorySimpleReadModel>> GetStoryRecommendations(string username);
    Task<StoryReadModel> GetStory(StoryId storyId);
    Task<List<GenreReadModel>> GetAllGenres();
}