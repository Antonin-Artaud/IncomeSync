using System.ComponentModel.DataAnnotations;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;

public class CreateTokenRequest : IRequest<TokenResponse>
{
    public string UserId { get; init; }
}