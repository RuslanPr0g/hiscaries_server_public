using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Application.Stories.Command;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Command;

public sealed class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, BaseResult>
{
    private readonly IStoryWriteService _service;

    public DeleteGenreCommandHandler(IStoryWriteService userService)
    {
        _service = userService;
    }

    public async Task<BaseResult> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        return await _service.DeleteGenre(request);
    }
}