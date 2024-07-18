using HC.Application.Interface;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Query;

public sealed class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<ReviewReadModel>>
{
    private readonly IUserReadService _userService;

    public GetReviewsQueryHandler(IUserReadService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<ReviewReadModel>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetReviewsFor(request.Username);
    }
}