using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Users.Command;

public class UpdateUserDataCommandHandler : IRequestHandler<UpdateUserDataCommand, BaseResult>
{
    private readonly IUserWriteService _userService;

    public UpdateUserDataCommandHandler(IUserWriteService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResult> Handle(UpdateUserDataCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUserData(request);
    }
}