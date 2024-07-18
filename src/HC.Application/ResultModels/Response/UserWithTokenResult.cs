namespace HC.Application.Models.Response;

public record UserWithTokenResult(ResultStatus ResultStatus, string FailReason, string Token, string RefreshToken)
    : BaseResult(ResultStatus, FailReason);