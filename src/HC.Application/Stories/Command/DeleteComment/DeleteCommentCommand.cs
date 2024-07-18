using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command;

public class DeleteCommentCommand : IRequest<BaseResult>
{
    public Guid Id { get; set; }
}