using System.Threading;
using System.Threading.Tasks;
using HC.Application.Common.Constants;
using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;

namespace HC.Application.Users.Query.BecomePublisher;

public class BecomePublisherCommandHandler : IRequestHandler<BecomePublisherCommand, BaseResult>
{
    private readonly IUserWriteService _userService;

    public BecomePublisherCommandHandler(IUserWriteService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResult> Handle(BecomePublisherCommand request, CancellationToken cancellationToken)
    {
        if (request.Username is null)
        {
            return BaseResult.CreateFail(UserFriendlyMessages.UsernameEmpty);
        }

        await _userService.BecomePublisher(request.Username);
        return BaseResult.CreateSuccess();
    }
}