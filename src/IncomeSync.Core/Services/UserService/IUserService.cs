using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using IncomeSync.Core.Shared.Exceptions.UserExceptions;

namespace IncomeSync.Core.Services.UserService;

public interface IUserService
{
    /// <summary>
    /// Create a new user, if an user is already exist with the mail then, throw an exception UserAlreadyExistException
    /// </summary>
    /// <param name="createUserRequest"><see cref="CreateUserRequest"/></param>
    /// <returns><see cref="UserResponse"/></returns>
    /// <exception cref="UserBaseAlreadyExistException"><see cref="UserBaseAlreadyExistException"/></exception>
    Task<UserResponse> CreateUserAsync(CreateUserRequest createUserRequest);
    Task<UserResponse?> GetUserByIdAsync(Guid createUserRequest);
    Task<UserResponse?> GetUserByEmailAsync(string email);
    Task<UserDeleteResponse> DeleteUserByAsync(CreateUserRequest createUserRequest);
}