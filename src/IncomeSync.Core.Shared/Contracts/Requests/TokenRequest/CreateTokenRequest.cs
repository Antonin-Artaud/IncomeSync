using System.ComponentModel.DataAnnotations;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using IncomeSync.Core.Shared.Entities.UserEntity;
using MediatR;

namespace IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;

public record CreateTokenRequest : IRequest<TokenResponse>
{
    public UserEntity User { get; init; } = default!;
}