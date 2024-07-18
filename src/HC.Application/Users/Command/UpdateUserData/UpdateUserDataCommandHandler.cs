using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Domain.User;
using MediatR;

namespace HC.Application.Users.Command;

public class UpdateUserDataCommandHandler : IRequestHandler<UpdateUserDataCommand, UpdateUserDataResult>
{
    private readonly IUserWriteService _userService;

    public UpdateUserDataCommandHandler(IUserWriteService userService)
    {
        _userService = userService;
    }

    public async Task<UpdateUserDataResult> Handle(UpdateUserDataCommand request, CancellationToken cancellationToken)
    {
        User userExists = await _userService.GetUserByUsername(request.Username);

        if (!string.IsNullOrEmpty(request.UpdatedUsername))
            userExists = await _userService.GetUserByUsername(request.UpdatedUsername);

        IList<User> users = await _userService.GetAllUsers();
        UpdateUserDataResult updateUserDataResult = new(ResultStatus.Success, string.Empty);

        if (userExists is null)
            return new UpdateUserDataResult(ResultStatus.Fail, "Something went wrong.");

        if (request.Email != userExists.Email && users.Any(u => u.Email == request.Email) && request.Banned == false)
            return new UpdateUserDataResult(ResultStatus.Fail, "Email already exists.");

        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            // TODO:
            // updateUserDataResult = await _userService.UpdateUserPassword(request.Username, request.PreviousPassword, request.NewPassword);
        }

        _ = await _userService.UpdateUserData(
            new User(0, 
                string.IsNullOrEmpty(request.UpdatedUsername) ? request.Username : request.UpdatedUsername,
                request.Email, )
        {
            Username = ,
            Email = ,
            BirthDate = request.BirthDate,
            Banned = request.Banned
        }, request.User);

        return updateUserDataResult;
    }
}