using System.ComponentModel.DataAnnotations;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.UserRequest;

public struct GetUserRequest : IRequest<UserResponse?>
{
    [Required(DisallowAllDefaultValues = true)]
    public Guid Id { get; init; }
}