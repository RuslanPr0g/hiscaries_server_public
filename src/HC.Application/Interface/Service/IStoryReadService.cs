using HC.Application.Stories.Query;
using HC.Domain.Stories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IStoryReadService
{
    Task<StoryReadModel> GetStoryById(StoryId storyId);
    Task<IEnumerable<GenreReadModel>> GetAllGenres();
    Task<IEnumerable<StoryReadModel>> GetStoryRecommendations(GetStoryRecommendationsQuery request);
    Task<IEnumerable<StoryReadModel>> SearchForStory(GetStoryListQuery request);
}