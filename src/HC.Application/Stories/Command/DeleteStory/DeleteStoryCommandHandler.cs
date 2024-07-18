using System.Threading;
using System.Threading.Tasks;
using HC.Application.Interface;
using MediatR;

namespace HC.Application.Stories.Command.DeleteStory;

public class DeleteStoryCommandHandler : IRequestHandler<DeleteStoryCommand, int>
{
    private readonly IStoryWriteService _storyService;

    public DeleteStoryCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<int> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.DeleteStory(new Story { Id = request.StoryId }, request.User);
    }
}