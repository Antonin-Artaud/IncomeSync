
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using IncomeSync.Core.Shared.Exceptions.UserExceptions;
using IncomeSync.Persistence.Repositories.UserRepository;

namespace IncomeSync.Core.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest createUserRequest)
    {
        var user = await _userRepository.FindUserByMailAsync(createUserRequest.Email);
        
        if (user is not null)
        {
            throw new UserAlreadyExistException("This email is already assigned to an user.");
        }
        
        return await _userRepository.InsertUserAsync(createUserRequest);
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid createUserRequest)
    {
        var user = await _userRepository.FindUserByIdAsync(createUserRequest);
        
        if (user is not null)
        {
            throw new UserAlreadyExistException("This email is already assigned to an user.");
        }

        return user;
    }

    public Task<UserDeleteResponse> DeleteUserByAsync(CreateUserRequest createUserRequest)
    {
        throw new NotImplementedException();
    }
}