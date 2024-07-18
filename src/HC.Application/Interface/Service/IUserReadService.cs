using HC.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IUserReadService
{
    Task<UserReadModel> GetUserById(UserId userId);
    Task<UserReadModel> GetUserByUsername(string username);
}