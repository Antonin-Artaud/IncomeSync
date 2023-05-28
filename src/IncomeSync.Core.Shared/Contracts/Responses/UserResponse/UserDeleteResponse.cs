using System.ComponentModel.DataAnnotations;

namespace IncomeSync.Core.Shared.Contracts.Responses.UserResponse;

public readonly struct UserDeleteResponse
{
    [Required(DisallowAllDefaultValues = true)]
    public Guid Id { get; init; }
}