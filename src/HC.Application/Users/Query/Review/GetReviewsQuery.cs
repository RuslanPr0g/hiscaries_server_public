using MediatR;
using System.Collections.Generic;

namespace HC.Application.Users.Query;

public sealed class GetReviewsQuery : IRequest<IEnumerable<ReviewReadModel>>
{
    public string Username { get; set; }
}