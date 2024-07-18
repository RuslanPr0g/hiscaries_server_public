using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command.ReadStory;

internal class ReadStoryCommandHandler : IRequestHandler<ReadStoryCommand, BaseResult>
{
    private readonly IStoryWriteService _storyService;

    public ReadStoryCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<BaseResult> Handle(ReadStoryCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.ReadStoryHistory(request);
    }
}