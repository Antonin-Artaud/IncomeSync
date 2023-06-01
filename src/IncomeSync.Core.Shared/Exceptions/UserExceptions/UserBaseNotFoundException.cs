namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserBaseNotFoundException : UserBaseException
{
    protected UserBaseNotFoundException()
    {
    }

    protected UserBaseNotFoundException(string? message) : base(message)
    {
    }

    protected UserBaseNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}