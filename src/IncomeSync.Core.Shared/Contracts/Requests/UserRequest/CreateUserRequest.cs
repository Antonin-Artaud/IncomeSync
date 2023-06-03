using System.ComponentModel.DataAnnotations;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.UserRequest;

public class CreateUserRequest : IRequest
{
    [Required(DisallowAllDefaultValues = true)]
    public string Email { get; init; } = null!;

    [Required(DisallowAllDefaultValues = true)]
    public string Password { get; init; } = null!;
}