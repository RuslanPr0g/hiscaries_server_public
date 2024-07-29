using HC.Application.Interface;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Query;

public class GetStoryListQueryHandler : IRequestHandler<GetStoryListQuery, IEnumerable<StorySimpleReadModel>>
{
    private readonly IStoryReadService _storySevice;

    public GetStoryListQueryHandler(IStoryReadService storyService)
    {
        _storySevice = storyService;
    }

    public async Task<IEnumerable<StorySimpleReadModel>> Handle(GetStoryListQuery request, CancellationToken cancellationToken)
    {
        return await _storySevice.SearchForStory(request);
    }
}