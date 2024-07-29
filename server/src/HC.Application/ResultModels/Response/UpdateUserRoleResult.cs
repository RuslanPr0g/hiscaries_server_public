namespace HC.Application.Models.Response;

public record UpdateUserRoleResult(ResultStatus ResultStatus, string FailReason)
    : BaseResult(ResultStatus, FailReason);