using System.ComponentModel.DataAnnotations;

namespace IncomeSync.Core.Shared.Contracts.Responses.UserResponse;

public readonly struct UserFailureResponse
{
    [Required(DisallowAllDefaultValues = true)]
    public IEnumerable<string> Errors { get; init; }
}