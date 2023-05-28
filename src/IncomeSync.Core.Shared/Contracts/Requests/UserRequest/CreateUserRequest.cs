using System.ComponentModel.DataAnnotations;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.UserRequest;

public class CreateUserRequest : IRequest<UserResponse>
{
    [Required(DisallowAllDefaultValues = true)]
    public string Email { get; init; } = default!;
}