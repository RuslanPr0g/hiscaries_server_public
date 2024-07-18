using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command;

public class BookmarkStoryCommand : IRequest<BaseResult>
{
    public Guid UserId { get; set; }
    public Guid StoryId { get; set; }
}