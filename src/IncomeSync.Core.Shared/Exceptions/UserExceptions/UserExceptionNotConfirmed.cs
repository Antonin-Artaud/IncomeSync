namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserNotConfirmed : UserBase
{
    protected UserNotConfirmed()
    {
    }

    protected UserNotConfirmed(string? message) : base(message)
    {
    }
}