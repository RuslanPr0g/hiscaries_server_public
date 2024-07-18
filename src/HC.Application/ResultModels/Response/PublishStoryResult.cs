namespace HC.Application.Models.Response;

public record PublishStoryResult(ResultStatus ResultStatus, string FailReason, int Id)
    : BaseResult(ResultStatus, FailReason);