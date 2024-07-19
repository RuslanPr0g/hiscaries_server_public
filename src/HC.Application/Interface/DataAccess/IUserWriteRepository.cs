using HC.Domain.Users;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IUserWriteRepository
{
    Task<User?> GetUserById(UserId userId);
    Task<User?> GetUserByUsername(string username);
    Task<int> AddUser(User user);
    Task BecomePublisher(string username);
    Task<RefreshToken> GetRefreshToken(string refreshToken);
    Task DeleteReview(Review review);
    Task<bool> IsUserExistByEmail(string email);
}