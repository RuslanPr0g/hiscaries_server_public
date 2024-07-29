using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command;

public class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, UpdateStoryInfoResult>
{
    private readonly IStoryWriteService _storyService;

    public CreateStoryCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<UpdateStoryInfoResult> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.PublishStory(request);
    }
}