namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserNotFoundException : UserBaseException
{
    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}