using System;

namespace HC.API.Requests;

public sealed class PublishReviewRequest
{
    public Guid Id { get; set; }
    public Guid PublisherId { get; set; }
    public Guid ReviewerId { get; set; }
    public string Message { get; set; }
}