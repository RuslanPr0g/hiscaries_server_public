using MediatR;
using System;
using System.Collections.Generic;

namespace HC.Application.Stories.Query;

public sealed class GetStoryListQuery : IRequest<IEnumerable<StoryReadModel>>
{
    public Guid? Id { get; set; }
    public string SearchTerm { get; set; }
    public string Genre { get; set; }
    public bool All { get; set; } = false;
}