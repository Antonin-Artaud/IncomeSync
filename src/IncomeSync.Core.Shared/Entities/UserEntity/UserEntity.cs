namespace IncomeSync.Core.Shared.Entities.UserEntity;

public class UserEntity
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string PasswordHash { get; set; }
}