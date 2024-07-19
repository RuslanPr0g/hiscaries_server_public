using HC.Application.Interface;
using HC.Domain.Users;
using HC.Infrastructure.DataAccess;
using System.Data.Entity;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public class EFUserWriteRepository : IUserWriteRepository
{
    private readonly HiscaryContext _context;

    public EFUserWriteRepository(HiscaryContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user) => await _context.Users.AddAsync(user);

    public async Task<User?> GetUserById(UserId userId) =>
        await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

    public async Task<User?> GetUserByUsername(string username) =>
        await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

    public async Task<bool> IsUserExistByEmail(string email) =>
        await _context.Users.AnyAsync(x => x.Email == email);
}

