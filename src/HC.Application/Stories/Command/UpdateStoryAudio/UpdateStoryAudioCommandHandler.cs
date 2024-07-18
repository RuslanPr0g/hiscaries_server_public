using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Application.Stories.Command.ScoreStory;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command;

public class UpdateStoryAudioCommandHandler : IRequestHandler<UpdateStoryAudioCommand, BaseResult>
{
    private readonly IStoryWriteService _storyService;

    public UpdateStoryAudioCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<BaseResult> Handle(UpdateStoryAudioCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.UpdateAudio(request);
    }
}