using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Application.Users.Command;
using HC.Application.Users.Command.LoginUser;
using HC.Application.Users.Command.PublishReview;
using HC.Application.Users.Command.RefreshToken;
using HC.Domain.Users;
using System.Threading.Tasks;

namespace HC.Application.Services;

public sealed class UserWriteService : IUserWriteService
{
    private readonly IUserWriteRepository _repository;

    public UserWriteService(IUserWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResult> BecomePublisher(string username)
    {
        User user = await _repository.GetUserByUsername(username);
        user.BecomePublisher();
        return BaseResult.CreateSuccess();
    }

    public Task<BaseResult> DeleteReview(DeleteReviewCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<User> GetUserById(UserId userId)
    {
        throw new System.NotImplementedException();
    }

    public Task<User> GetUserByUsername(string username)
    {
        throw new System.NotImplementedException();
    }

    public Task<LoginUserResult> LoginUser(LoginUserCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> PublishReview(PublishReviewCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<RefreshTokenResponse> RefreshToken(RefreshTokenCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<UserWithTokenResult> RegisterUser(RegisterUserCommand command)
    {
        throw new System.NotImplementedException();
    }

    public Task<BaseResult> UpdateUserData(UpdateUserDataCommand command)
    {
        throw new System.NotImplementedException();
    }
}
