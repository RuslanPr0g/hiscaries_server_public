using MediatR;
using System.Collections.Generic;

namespace HC.Application.Stories.Query;

public sealed class GetStoryRecommendationsQuery : IRequest<IEnumerable<StorySimpleReadModel>>
{
    public string Username { get; set; }
}