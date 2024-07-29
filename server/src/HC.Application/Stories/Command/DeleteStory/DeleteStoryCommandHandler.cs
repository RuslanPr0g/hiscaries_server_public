using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command.DeleteStory;

public class DeleteStoryCommandHandler : IRequestHandler<DeleteStoryCommand, BaseResult>
{
    private readonly IStoryWriteService _storyService;

    public DeleteStoryCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<BaseResult> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.DeleteStory(request);
    }
}