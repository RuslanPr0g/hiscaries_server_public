using HC.Application.Interface;
using HC.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Application.Services;

public sealed class UserReadService : IUserReadService
{
    public Task<IEnumerable<ReviewReadModel>> GetReviewsFor(string username)
    {
        throw new System.NotImplementedException();
    }

    public Task<UserReadModel> GetUserById(UserId userId)
    {
        throw new System.NotImplementedException();
    }

    public Task<UserReadModel> GetUserByUsername(string username)
    {
        throw new System.NotImplementedException();
    }
}
