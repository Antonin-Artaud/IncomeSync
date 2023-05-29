using System.ComponentModel.DataAnnotations;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;

public readonly struct CreateTokenRequest : IRequest<TokenResponse>
{
    [Required(DisallowAllDefaultValues = true)]
    public string Email { get; init; }
    
    [Required(DisallowAllDefaultValues = true)]
    public string Password { get; init; }
    
}