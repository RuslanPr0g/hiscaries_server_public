using HC.Application.Interface;
using HC.Domain.Users;
using HC.Infrastructure.DataAccess;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public class EFUserWriteRepository : IUserWriteRepository
{
    private readonly HiscaryContext _context;

    public EFUserWriteRepository(HiscaryContext context)
    {
        _context = context;
    }

    public Task<int> AddUser(User user)
    {
        throw new System.NotImplementedException();
    }

    public Task BecomePublisher(string username)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteReview(Review review)
    {
        throw new System.NotImplementedException();
    }

    public Task<RefreshToken> GetRefreshToken(string refreshToken)
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

    public Task<string> GetUserRoleByUsername(string username)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateRefreshToken(RefreshToken refreshToken)
    {
        throw new System.NotImplementedException();
    }
}

