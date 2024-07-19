namespace HC.Application.Models.Response;

public record UserWithTokenResult(
    ResultStatus ResultStatus,
    string? Token,
    string? RefreshToken,
    string? FailReason = null)
    : BaseResult(ResultStatus, FailReason)
{
    public static UserWithTokenResult CreateSuccess(string Token, string RefreshToken)
    {
        return new UserWithTokenResult(ResultStatus.Success, Token, RefreshToken);
    }

    public new static UserWithTokenResult CreateFail(string reason)
    {
        return new UserWithTokenResult(ResultStatus.Fail, null, null, reason);
    }
}