using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command.ScoreStory;

public class UpdateStoryAudioCommand : IRequest<BaseResult>
{
    public Guid StoryId { get; set; }
    public string Name { get; set; }
    public byte[] Audio { get; set; }
}