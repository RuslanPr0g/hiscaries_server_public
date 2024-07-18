namespace HC.Application.Models.Response;

public record AddStoryPageResult(ResultStatus ResultStatus, string FailReason, int StoryPageId)
    : BaseResult(ResultStatus, FailReason);