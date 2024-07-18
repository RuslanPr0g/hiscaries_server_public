using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command.DeleteStory;

public class DeleteStoryAudioCommandHandler : IRequestHandler<DeleteStoryAudioCommand, BaseResult>
{
    private readonly IStoryWriteService _storyService;

    public DeleteStoryAudioCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<BaseResult> Handle(DeleteStoryAudioCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.DeleteAudio(request);
    }
}