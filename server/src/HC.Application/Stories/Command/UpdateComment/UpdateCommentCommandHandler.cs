using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command.UpdateComment;

internal class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, BaseResult>
{
    private readonly IStoryWriteService _storyService;

    public UpdateCommentCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<BaseResult> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.UpdateComment(request);
    }
}