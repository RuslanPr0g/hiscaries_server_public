﻿using System;
using HC.Application.Models.Response;
using MediatR;

namespace HC.Application.Users.Command;

public class CreateUserCommand : IRequest<RegisterUserResult>
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public DateTime BirthDate { get; set; }
}