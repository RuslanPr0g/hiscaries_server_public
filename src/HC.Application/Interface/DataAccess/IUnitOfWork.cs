using System.Threading.Tasks;

namespace HC.Application.Interface.DataAccess;

/// <summary>
/// EF Core already implements the unit of work pattern.
/// This interface is used, so that we can save changes easily
/// without depending on a db context (we might use many contexts).
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChanges();
}
