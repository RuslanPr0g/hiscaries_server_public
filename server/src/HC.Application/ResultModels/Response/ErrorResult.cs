using System.Collections.Generic;
using HC.Application.Models.Message;

namespace HC.Application.Models.Response;

public record ErrorResult(List<ErrorMessage> Errors);