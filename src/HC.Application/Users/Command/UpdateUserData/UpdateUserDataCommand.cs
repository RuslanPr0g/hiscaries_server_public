using HC.Application.Models.Response;
using MediatR;
using System;

namespace HC.Application.Users.Command;

public sealed class UpdateUserDataCommand : IRequest<BaseResult>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string UpdatedUsername { get; set; }
    public DateTime BirthDate { get; set; }

    public string PreviousPassword { get; set; }
    public string NewPassword { get; set; }
}