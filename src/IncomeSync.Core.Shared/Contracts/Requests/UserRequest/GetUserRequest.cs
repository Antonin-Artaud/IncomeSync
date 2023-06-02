using System.ComponentModel.DataAnnotations;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.UserRequest;

public record GetUserRequest : IRequest<UserResponse?>
{
    [Required(DisallowAllDefaultValues = true)]
    public Guid Id { get; init; }
}