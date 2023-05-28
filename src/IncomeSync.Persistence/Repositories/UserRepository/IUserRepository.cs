using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;

namespace IncomeSync.Persistence.Repositories.UserRepository;

public interface IUserRepository
{
    Task<UserResponse> InsertUserAsync(CreateUserRequest createUserRequest);
    Task<UserResponse?> FindUserByIdAsync(Guid id);
    Task<UserResponse?> FindUserByMailAsync(string email);
    Task<UserDeleteResponse> DeleteUserByAsync(CreateUserRequest createUserRequest);
}