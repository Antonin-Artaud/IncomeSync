using Microsoft.IdentityModel.Tokens;

namespace IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;

public readonly struct TokenResponse
{
    public string Id { get; init; }

    public string Issuer { get; init; }

    public SecurityKey SecurityKey { get; init; }

    public SecurityKey SigningKey { get; init; }

    public DateTime ValidFrom { get; init; }

    public DateTime ValidTo { get; init; }
}