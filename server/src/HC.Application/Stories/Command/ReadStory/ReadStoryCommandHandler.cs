using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command.ReadStory;

internal class ReadStoryCommandHandler : IRequestHandler<ReadStoryCommand, BaseResult>
{
    private readonly IUserWriteService _service;

    public ReadStoryCommandHandler(IUserWriteService storyService)
    {
        _service = storyService;
    }

    public async Task<BaseResult> Handle(ReadStoryCommand request, CancellationToken cancellationToken)
    {
        return await _service.ReadStoryHistory(request);
    }
}