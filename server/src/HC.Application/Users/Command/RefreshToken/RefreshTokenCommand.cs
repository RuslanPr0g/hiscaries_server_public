using HC.Application.Models.Response;
using MediatR;

namespace HC.Application.Users.Command.RefreshToken;

public class RefreshTokenCommand : IRequest<UserWithTokenResult>
{
    public string Username { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}