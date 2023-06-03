﻿using System.Runtime.Serialization;

namespace IncomeSync.Core.Shared.Exceptions.UserExceptions;

public class UserAlreadyExistException : UserBaseException
{
    public UserAlreadyExistException()
    {
    }

    public UserAlreadyExistException(string? message) : base(message)
    {
    }

    public UserAlreadyExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}