using System.Runtime.Serialization;

namespace IncomeSync.Core.Shared.Exceptions;

public class PublicKeyNotFoundException : Exception
{
    public PublicKeyNotFoundException()
    {
    }

    public PublicKeyNotFoundException(string? message) : base(message)
    {
    }
}