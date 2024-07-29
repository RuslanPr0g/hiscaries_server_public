using HC.Application.Models.Response;
using MediatR;

namespace HC.Application.Users.Command.LoginUser;

public class LoginUserCommand : IRequest<UserWithTokenResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}