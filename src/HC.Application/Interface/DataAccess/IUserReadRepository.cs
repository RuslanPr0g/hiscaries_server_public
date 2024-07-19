using System;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IUserReadRepository
{
    Task<UserReadModel?> GetUserById(Guid userId);
    Task<UserReadModel?> GetUserByUsername(string username);
}