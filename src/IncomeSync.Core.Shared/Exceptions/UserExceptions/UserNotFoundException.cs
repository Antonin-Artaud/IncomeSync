namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserNotFoundException : UserException
{
    protected UserNotFoundException()
    {
    }

    protected UserNotFoundException(string? message) : base(message)
    {
    }

    protected UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}