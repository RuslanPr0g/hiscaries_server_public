using HC.Application.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Query;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserReadModel>
{
    private readonly IUserReadService _userService;

    public GetUserInfoQueryHandler(IUserReadService userService)
    {
        _userService = userService;
    }

    public async Task<UserReadModel> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetUserByUsername(request.Username);
    }
}