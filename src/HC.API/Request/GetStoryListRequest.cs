using System;

namespace HC.API.Request;

public sealed class GetStoryListRequest
{
    public Guid Id { get; set; }
    public string Search { get; set; }
    public string Genre { get; set; }
}