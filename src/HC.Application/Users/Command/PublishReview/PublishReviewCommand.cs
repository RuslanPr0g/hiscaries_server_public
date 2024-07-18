using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Users.Command.PublishReview;

public sealed class PublishReviewCommand : IRequest<BaseResult>
{
    public Guid Id { get; set; }
    public Guid PublisherId { get; set; }
    public Guid ReviewerId { get; set; }
    public string? Message { get; set; }
}