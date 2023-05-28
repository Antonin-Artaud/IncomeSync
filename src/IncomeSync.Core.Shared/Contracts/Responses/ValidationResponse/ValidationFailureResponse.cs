namespace IncomeSync.Core.Shared.Contracts.Responses.ValidationResponse;

public readonly struct ValidationFailureResponse
{
    public IEnumerable<string> Errors { get; init; }
}