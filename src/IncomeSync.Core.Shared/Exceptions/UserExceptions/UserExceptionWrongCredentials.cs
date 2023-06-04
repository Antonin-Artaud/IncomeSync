namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserWrongCredentials : UserBase
{
    public UserWrongCredentials()
    {
    }

    public UserWrongCredentials(string? message) : base(message)
    {
    }

    public UserWrongCredentials(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}