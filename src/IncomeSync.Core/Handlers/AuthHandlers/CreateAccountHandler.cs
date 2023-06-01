using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using MediatR;

namespace IncomeSync.Core.Handlers.AuthHandlers;

public class RegisterHandler : IRequestHandler<CreateAccountRequest, TokenResponse>
{
    public Task<TokenResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        return 
    }
}