using HC.Application.Interface;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Query;

public class GetStoryListQueryHandler : IRequestHandler<GetStoryListQuery, IEnumerable<StoryReadModel>>
{
    private readonly IStoryReadService _storySevice;

    public GetStoryListQueryHandler(IStoryReadService storyService)
    {
        _storySevice = storyService;
    }

    public async Task<IEnumerable<StoryReadModel>> Handle(GetStoryListQuery request, CancellationToken cancellationToken)
    {
        return await _storySevice.SearchForStory(request);
    }
}