namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

[Serializable]
public class UserBaseException : Exception
{
    protected UserBaseException()
    {
    }

    protected UserBaseException(string? message) : base(message)
    {
    }

    protected UserBaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}