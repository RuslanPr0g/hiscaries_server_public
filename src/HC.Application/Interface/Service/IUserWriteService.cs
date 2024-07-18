using HC.Application.Models.Response;
using HC.Application.Users.Command;
using HC.Application.Users.Command.LoginUser;
using HC.Application.Users.Command.PublishReview;
using HC.Application.Users.Command.RefreshToken;
using HC.Domain.Users;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IUserWriteService
{
    Task<User> GetUserById(UserId userId);
    Task<User> GetUserByUsername(string username);
    Task<BaseResult> BecomePublisher(string username);

    Task<UserWithTokenResult> RegisterUser(RegisterUserCommand command);
    Task<LoginUserResult> LoginUser(LoginUserCommand command);

    Task<BaseResult> PublishReview(PublishReviewCommand command);
    Task<BaseResult> DeleteReview(DeleteReviewCommand command);
    Task<BaseResult> UpdateUserData(UpdateUserDataCommand command);
    Task<RefreshTokenResponse> RefreshToken(RefreshTokenCommand command);
}