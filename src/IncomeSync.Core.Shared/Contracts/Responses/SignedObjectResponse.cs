namespace IncomeSync.Core.Shared.Contracts.Responses;

public record SignedObjectResponse
{
    public string Data { get; init; } = default!;
    public string Signature { get; init; } = default!;
}