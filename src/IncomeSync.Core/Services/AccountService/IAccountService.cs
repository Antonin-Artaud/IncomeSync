using IncomeSync.Core.Shared.Entities.UserEntity;

namespace IncomeSync.Core.Services.AccountService;

public interface IAccountService
{
    Task RegisterUser(UserEntity userEntity, string password);
    Task<bool> VerifyPassword();
}