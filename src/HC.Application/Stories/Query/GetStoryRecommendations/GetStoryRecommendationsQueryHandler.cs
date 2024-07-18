using HC.Application.Interface;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Query;

public class GetStoryRecommendationsQueryHandler : IRequestHandler<GetStoryRecommendationsQuery, IEnumerable<StoryReadModel>>
{
    private readonly IStoryReadService _storySevice;

    public GetStoryRecommendationsQueryHandler(IStoryReadService storyService)
    {
        _storySevice = storyService;
    }

    public async Task<IEnumerable<StoryReadModel>> Handle(GetStoryRecommendationsQuery request, CancellationToken cancellationToken)
    {
        return await _storySevice.GetStoryRecommendations(request);
    }
}