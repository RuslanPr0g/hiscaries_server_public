using HC.Application.Interface.DataAccess;
using HC.Infrastructure.DataAccess;
using System.Threading.Tasks;

namespace HC.Infrastructure.UnitOfWorks;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly HiscaryContext _context;

    public UnitOfWork(HiscaryContext context)
    {
        _context = context;
    }

    public Task<int> SaveChanges() => _context.SaveChangesAsync();
}
