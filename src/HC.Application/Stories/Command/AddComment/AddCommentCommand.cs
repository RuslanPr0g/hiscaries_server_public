using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command;

public class AddCommentCommand : IRequest<BaseResult>
{
    public string Username { get; init; }
    public string Content { get; init; }
    public DateTime CommentedAt { get; init; }
    public int Score { get; init; }
    public Guid StoryId { get; set; }
    public Guid UserId { get; set; }
}