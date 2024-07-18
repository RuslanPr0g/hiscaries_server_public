using HC.Application.Interface;
using HC.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;
public class EFUserReadRepository : IUserReadRepository
{
    private readonly HiscaryContext _context;

    public EFUserReadRepository(HiscaryContext context)
    {
        _context = context;
    }

    public Task<UserReadModel> GetUserById(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserReadModel> GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserReadModel>> GetUsers()
    {
        throw new NotImplementedException();
    }
}

