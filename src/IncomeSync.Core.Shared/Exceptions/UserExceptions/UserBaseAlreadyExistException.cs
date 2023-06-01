using System.Runtime.Serialization;

namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserBaseAlreadyExistException : UserBaseException
{
    public UserBaseAlreadyExistException()
    {
    }

    public UserBaseAlreadyExistException(string? message) : base(message)
    {
    }

    public UserBaseAlreadyExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}