namespace HC.Application.Models.Response;

public record UpdateStoryInfoResult(ResultStatus ResultStatus, string FailReason, int Id)
    : BaseResult(ResultStatus, FailReason);