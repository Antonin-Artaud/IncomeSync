using Microsoft.IdentityModel.Tokens;

namespace IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;

public readonly struct TokenResponse
{
    public string Token { get; init; }
}