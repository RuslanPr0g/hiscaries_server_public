using MediatR;

namespace HC.Application.Users.Query;

public sealed class GetUserInfoQuery : IRequest<UserReadModel>
{
    public string Username { get; set; }
}