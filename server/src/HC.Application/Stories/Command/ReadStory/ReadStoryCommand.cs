using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command.ReadStory;

public class ReadStoryCommand : IRequest<BaseResult>
{
    public Guid StoryId { get; set; }
    public Guid UserId { get; set; }
    public int Page { get; set; }
}