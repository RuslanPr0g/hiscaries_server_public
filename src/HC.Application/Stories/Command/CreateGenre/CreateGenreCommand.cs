using HC.Application.Models.Response;
using MediatR;

namespace HC.Application.Stories.Command;

public sealed class CreateGenreCommand : IRequest<BaseResult>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] ImagePreview { get; set; }
}