using HC.Application.Interface;
using HC.Domain.Stories;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public sealed class EFStoryWriteRepository : IStoryWriteRepository
{
    public Task<int> AddStory(Story story)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> DeleteStory(StoryId storyId)
    {
        throw new System.NotImplementedException();
    }

    public Task<Story> GetStory(StoryId storyId)
    {
        throw new System.NotImplementedException();
    }
}