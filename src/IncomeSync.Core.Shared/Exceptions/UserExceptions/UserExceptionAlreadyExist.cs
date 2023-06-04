using System.Runtime.Serialization;

namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserExceptionAlreadyExist : UserExceptionBase
{
    public UserExceptionAlreadyExist()
    {
    }

    public UserExceptionAlreadyExist(string? message) : base(message)
    {
    }

    public UserExceptionAlreadyExist(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}