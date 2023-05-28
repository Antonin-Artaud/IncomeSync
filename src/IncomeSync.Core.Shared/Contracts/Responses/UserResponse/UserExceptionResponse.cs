using System.ComponentModel.DataAnnotations;

namespace IncomeSync.Core.Shared.Contracts.Responses.UserResponse;

public readonly struct UserExceptionResponse
{
    [Required(DisallowAllDefaultValues = true)]
    public string ErrorMessage { get; init; }
}