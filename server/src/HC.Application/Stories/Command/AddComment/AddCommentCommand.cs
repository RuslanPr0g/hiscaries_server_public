using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command;

public class AddCommentCommand : IRequest<BaseResult>
{
    public Guid StoryId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; init; }
    public int Score { get; init; }
}