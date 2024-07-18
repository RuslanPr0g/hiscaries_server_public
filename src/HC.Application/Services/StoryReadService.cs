using HC.Application.Interface;
using HC.Application.Stories.Query;
using HC.Domain.Stories;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class StoryReadService : IStoryReadService
{
    public Task<IEnumerable<GenreReadModel>> GetAllGenres()
    {
        throw new System.NotImplementedException();
    }

    public Task<StoryReadModel> GetStoryById(StoryId storyId)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<StoryReadModel>> GetStoryRecommendations(GetStoryRecommendationsQuery request)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<StoryReadModel>> SearchForStory(GetStoryListQuery request)
    {
        throw new System.NotImplementedException();
    }
}