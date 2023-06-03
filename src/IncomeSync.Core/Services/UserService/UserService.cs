
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using IncomeSync.Core.Shared.Entities.UserEntity;
using IncomeSync.Core.Shared.Exceptions.UserExceptions;
using IncomeSync.Persistence.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;

namespace IncomeSync.Core.Services.UserService;

public class UserService : IUserService
{
    private readonly PasswordHasher<UserEntity> _passwordHasher;
    private readonly IUserRepository _userRepository;


    public UserService(IUserRepository userRepository)
    {
        _passwordHasher = new PasswordHasher<UserEntity>();
        _userRepository = userRepository;
    }

    public async Task CreateUserAsync(CreateUserRequest request)
    {
        var response = await _userRepository.FindUserByMailAsync(request.Email);
        
        if (response is not null)
        {
            throw new UserAlreadyExistException("This email is already assigned to an user.");
        }

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Email = request.Email
        };
        
        var passwordHash = _passwordHasher.HashPassword(user, request.Password);

        user.PasswordHash = passwordHash;
        
        await _userRepository.InsertUserAsync(user);
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid createUserRequest)
    {
        var response = await _userRepository.FindUserByIdAsync(createUserRequest);
        
        if (response is null)
        {
            return null;
        }
        
        return new UserResponse
        {
            Id = response.Id,
            Email = response.Email
        };
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        var response = await _userRepository.FindUserByMailAsync(email);

        if (response is null)
        {
            return null;
        }
        
        return new UserResponse
        {
            Id = response.Id,
            Email = response.Email
        };
    }

    public async Task<UserResponse?> GetUserByCredentialsAsync(string email, string password)
    {
        var response = await _userRepository.FindUserByMailAsync(email);

        if (response is null)
        {
            throw new UserNotFoundException("This email is already assigned to an user.");
        }
        
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(response, response.PasswordHash, password);
        
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new UserCredentialsException("The email or password is incorrect.");
        }
        
        return new UserResponse
        {
            Id = response.Id,
            Email = response.Email
        };
    }

    public async Task<UserDeleteResponse> DeleteUserByAsync(CreateUserRequest request)
    {
        var response = await _userRepository.FindUserByMailAsync(request.Email);
        
        if (response is null)
        {
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteUserByAsync(response!);

        return new UserDeleteResponse()
        {
            Id = response.Id
        };
    }
}