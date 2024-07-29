using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command;

public sealed class DeleteGenreCommand : IRequest<BaseResult>
{
    public Guid GenreId { get; set; }
}