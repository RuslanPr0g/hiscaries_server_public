using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Stories.Command.ScoreStory;

public class StoryScoreCommand : IRequest<BaseResult>
{
    public Guid StoryId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
}