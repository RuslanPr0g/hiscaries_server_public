using HC.Application.Interface;
using HC.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public class EFUserReadRepository : IUserReadRepository
{
    private readonly HiscaryContext _context;

    public EFUserReadRepository(HiscaryContext context)
    {
        _context = context;
    }

    public async Task<UserReadModel?> GetUserById(Guid userId) =>
        await _context.Users
            .Where(x => x.Id == userId)
            .Select(user => UserReadModel.FromDomainModel(user))
            .FirstOrDefaultAsync();

    public async Task<UserReadModel?> GetUserByUsername(string username) =>
        await _context.Users
            .Where(x => x.Username == username)
            .Select(user => UserReadModel.FromDomainModel(user))
            .FirstOrDefaultAsync();
}

