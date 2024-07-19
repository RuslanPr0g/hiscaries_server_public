using HC.Application.Interface;
using HC.Domain.Stories;
using HC.Infrastructure.DataAccess;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public sealed class EFStoryWriteRepository : IStoryWriteRepository
{
    private readonly HiscaryContext _context;

    public EFStoryWriteRepository(HiscaryContext context)
    {
        _context = context;
    }

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