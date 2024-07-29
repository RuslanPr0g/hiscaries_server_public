using HC.Application.Interface;
using HC.Domain.Users;
using System.Threading.Tasks;

namespace HC.Application.Services;

public sealed class UserReadService : IUserReadService
{
    private readonly IUserReadRepository _repository;

    public UserReadService(IUserReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserReadModel> GetUserById(UserId userId) => await _repository.GetUserById(userId);

    public async Task<UserReadModel> GetUserByUsername(string username) => await _repository.GetUserByUsername(username);
}
