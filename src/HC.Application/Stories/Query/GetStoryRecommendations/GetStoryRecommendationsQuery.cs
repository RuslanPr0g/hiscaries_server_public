using MediatR;
using System.Collections.Generic;

namespace HC.Application.Stories.Query;

public sealed class GetStoryRecommendationsQuery : IRequest<IEnumerable<StoryReadModel>>
{
    public string Username { get; set; }
}