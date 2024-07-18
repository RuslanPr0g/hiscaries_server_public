using HC.Application.Models.Response;
using MediatR;

namespace HC.Application.Users.Command.RefreshToken;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}