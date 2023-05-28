using System.ComponentModel.DataAnnotations;

namespace IncomeSync.Core.Shared.Contracts.Responses.UserResponse;

public readonly struct UserResponse
{
    [Required(DisallowAllDefaultValues = true)]
    public Guid Id { get; init; }
    
    [Required(DisallowAllDefaultValues = true)]
    public string Email { get; init; }
}