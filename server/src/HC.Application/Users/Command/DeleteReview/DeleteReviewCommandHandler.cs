using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Command;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, BaseResult>
{
    private readonly IUserWriteService _userService;

    public DeleteReviewCommandHandler(IUserWriteService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResult> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        return await _userService.DeleteReview(request);
    }
}