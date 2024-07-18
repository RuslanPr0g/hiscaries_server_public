using System;

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

    internal static BaseResult CreateFail(object userWasNotFound)
    {
        throw new NotImplementedException();
    }
}