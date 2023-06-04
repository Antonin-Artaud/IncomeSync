namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

[Serializable]
public class UserBase : Exception
{
    protected UserBase()
    {
    }

    protected UserBase(string? message) : base(message)
    {
    }

    protected UserBase(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}