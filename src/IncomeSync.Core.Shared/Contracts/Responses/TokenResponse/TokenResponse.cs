namespace IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;

public record TokenResponse
{
    public string Token { get; init; } = default!;
}