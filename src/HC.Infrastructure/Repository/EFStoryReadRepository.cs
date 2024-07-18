using HC.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public sealed class EFStoryReadRepository : IStoryReadRepository
{
    public Task<List<GenreReadModel>> GetGenres()
    {
        throw new System.NotImplementedException();
    }

    public Task<IReadOnlyCollection<StoryReadModel>> GetStories()
    {
        throw new System.NotImplementedException();
    }

    public Task<StoryReadModel> GetStory(int storyId)
    {
        throw new System.NotImplementedException();
    }
}
