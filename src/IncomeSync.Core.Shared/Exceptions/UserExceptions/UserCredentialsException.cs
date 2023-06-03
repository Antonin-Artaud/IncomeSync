namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserCredentialsException : UserBaseException
{
    public UserCredentialsException()
    {
    }

    public UserCredentialsException(string? message) : base(message)
    {
    }

    public UserCredentialsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}