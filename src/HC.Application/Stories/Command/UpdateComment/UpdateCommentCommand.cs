using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command;

public class UpdateCommentCommand : IRequest<BaseResult>
{
    public Guid Id { get; set; }
    public Guid StoryId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
}