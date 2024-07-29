using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Application.Stories.Command;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Command;

public sealed class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, BaseResult>
{
    private readonly IStoryWriteService _service;

    public UpdateGenreCommandHandler(IStoryWriteService userService)
    {
        _service = userService;
    }

    public async Task<BaseResult> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        return await _service.UpdateGenre(request);
    }
}