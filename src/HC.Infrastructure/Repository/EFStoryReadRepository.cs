using HC.Application.Interface;
using HC.Domain.Stories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public sealed class EFStoryReadRepository : IStoryReadRepository
{
    public Task<List<GenreReadModel>> GetAllGenres()
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<StorySimpleReadModel>> GetStories()
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<StorySimpleReadModel>> GetStoriesBy(string searchTerm, string genre)
    {
        throw new System.NotImplementedException();
    }

    public Task<StoryReadModel> GetStory(StoryId storyId)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<StorySimpleReadModel>> GetStoryRecommendations(string username)
    {
        throw new System.NotImplementedException();
    }
}
