using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Command.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserWithTokenResult>
{
    private readonly IUserWriteService _userService;

    public LoginUserCommandHandler(IUserWriteService userService)
    {
        _userService = userService;
    }

    public async Task<UserWithTokenResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.LoginUser(request);
    }
}