namespace IncomeSync.Core.Shared.Exceptions;

public class PrivateKeyNotFoundException : Exception
{
    public PrivateKeyNotFoundException()
    {
    }

    public PrivateKeyNotFoundException(string? message) : base(message)
    {
    }
}