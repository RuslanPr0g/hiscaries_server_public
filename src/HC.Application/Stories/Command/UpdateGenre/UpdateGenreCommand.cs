using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command;

public sealed class UpdateGenreCommand : IRequest<BaseResult>
{
    public Guid GenreId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] ImagePreview { get; set; }
}