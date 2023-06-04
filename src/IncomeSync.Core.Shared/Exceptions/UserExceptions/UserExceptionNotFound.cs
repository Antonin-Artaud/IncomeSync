namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserNotFound : UserBase
{
    public UserNotFound()
    {
    }

    public UserNotFound(string? message) : base(message)
    {
    }

    public UserNotFound(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}