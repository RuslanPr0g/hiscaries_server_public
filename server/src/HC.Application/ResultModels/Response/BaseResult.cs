namespace HC.Application.Models.Response;

public record BaseResult(ResultStatus ResultStatus, string? FailReason = null)
{
    public static BaseResult CreateSuccess()
    {
        return new BaseResult(ResultStatus.Success);
    }

    public static BaseResult CreateFail(string reason)
    {
        return new BaseResult(ResultStatus.Fail, reason);
    }
}

public record BaseResult<T>(ResultStatus ResultStatus, T? Value, string? FailReason = null)
    where T : class
{
    public static BaseResult<T> CreateSuccess(T value)
    {
        return new BaseResult<T>(ResultStatus.Success, value);
    }

    public static BaseResult<T> CreateFail(string reason)
    {
        return new BaseResult<T>(ResultStatus.Fail, default, reason);
    }
}