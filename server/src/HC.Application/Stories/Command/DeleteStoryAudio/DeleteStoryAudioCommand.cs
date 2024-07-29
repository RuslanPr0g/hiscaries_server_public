using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command.DeleteStory;

public class DeleteStoryAudioCommand : IRequest<BaseResult>
{
    public Guid StoryId { get; set; }
}