namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

[Serializable]
public class UserException : Exception
{
    protected UserException()
    {
    }

    protected UserException(string? message) : base(message)
    {
    }

    protected UserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}