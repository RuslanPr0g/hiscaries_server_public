using System;

namespace HC.Application.Models.Response;

public record UpdateStoryInfoResult(ResultStatus ResultStatus, Guid? StoryId, string? FailReason = null)
    : BaseResult(ResultStatus, FailReason)
{
    public static UpdateStoryInfoResult CreateSuccess(Guid storyId)
    {
        return new UpdateStoryInfoResult(ResultStatus.Success, storyId);
    }

    public new static UpdateStoryInfoResult CreateFail(string reason)
    {
        return new UpdateStoryInfoResult(ResultStatus.Fail, null, reason);
    }
}
