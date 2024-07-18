using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, BaseResult>
{
    private readonly IStoryWriteService _storyService;

    public AddCommentCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<BaseResult> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.AddComment(request);
    }
}