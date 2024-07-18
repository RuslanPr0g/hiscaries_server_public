using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Application.Models.Message;
using HC.Application.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HC.Application.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            KeyValuePair<string, IEnumerable<string>>[] errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

            ErrorResult errorResponse = new ErrorResult(new List<ErrorMessage>());

            foreach (KeyValuePair<string, IEnumerable<string>> error in errors)
            foreach (string subError in error.Value)
            {
                ErrorMessage errorModel = new ErrorMessage
                {
                    FieldName = error.Key,
                    Message = subError
                };

                errorResponse.Errors.Add(errorModel);
            }

            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}