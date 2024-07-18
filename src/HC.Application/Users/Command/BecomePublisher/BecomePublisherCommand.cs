using HC.Application.Models.Response;
using MediatR;

namespace HC.Application.Users.Query.BecomePublisher;

public sealed class BecomePublisherCommand : IRequest<BaseResult>
{
    public string? Username { get; set; }
}