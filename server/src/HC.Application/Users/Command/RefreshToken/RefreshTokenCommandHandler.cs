﻿using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Command.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, UserWithTokenResult>
{
    private readonly IUserWriteService _userService;

    public RefreshTokenCommandHandler(IUserWriteService userService)
    {
        _userService = userService;
    }

    public async Task<UserWithTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RefreshToken(request);
    }
}