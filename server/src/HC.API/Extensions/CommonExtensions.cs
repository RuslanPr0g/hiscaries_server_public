using HC.Application.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HC.API.Extensions;

public static class CommonExtensions
{
    public static IActionResult ToObjectResult(this BaseResult result) => result.ResultStatus switch
    {
        ResultStatus.Success => new OkObjectResult(result),
        ResultStatus.Fail => new BadRequestObjectResult(result),
        // TODO: add not found, etc.
        _ => new NoContentResult()
    };
}
