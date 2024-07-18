using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command;

public sealed class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, BaseResult>
{
    private readonly IStoryWriteService _service;

    public CreateGenreCommandHandler(IStoryWriteService userService)
    {
        _service = userService;
    }

    public async Task<BaseResult> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        return await _service.CreateGenre(request);
    }
}