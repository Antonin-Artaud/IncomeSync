using System.ComponentModel.DataAnnotations;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;

public class CreateAccountRequest : IRequest<TokenResponse>
{
    [Required(DisallowAllDefaultValues = true)]
    public string Email { get; init; } = string.Empty;

    [Required(DisallowAllDefaultValues = true)]
    public string Password { get; init; } = string.Empty;
}