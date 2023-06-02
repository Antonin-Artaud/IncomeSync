using IncomeSync.Core.Services.UserService;
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Entities.UserEntity;
using Microsoft.AspNetCore.Identity;

namespace IncomeSync.Core.Services.AccountService;

public class AccountService : IAccountService
{
    private readonly PasswordHasher<UserEntity> _passwordHasher;
    private readonly IUserService _userService;
    
    public AccountService(PasswordHasher<UserEntity> passwordHasher, IUserService userService)
    {
        _passwordHasher = passwordHasher;
        _userService = userService;
    }
    
    public async Task RegisterUser(UserEntity userEntity, string password)
    {
        userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, password);
        await _userService.CreateUserAsync(new CreateUserRequest()
        {
            Email = userEntity.Email,
            Password = userEntity.PasswordHash
        });
    }

    public Task<bool> VerifyPassword()
    {
        throw new NotImplementedException();
    }
}