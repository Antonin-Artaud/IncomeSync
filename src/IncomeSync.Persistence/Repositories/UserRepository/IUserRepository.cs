using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using IncomeSync.Core.Shared.Entities.UserEntity;

namespace IncomeSync.Persistence.Repositories.UserRepository;

public interface IUserRepository
{
    Task InsertUserAsync(UserEntity userEntity);
    Task<UserEntity?> FindUserByIdAsync(Guid id);
    Task<UserEntity?> FindUserByMailAsync(string email);
    Task DeleteUserByAsync(UserEntity userEntity);
}