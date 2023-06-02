using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;
using IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using MediatR;

namespace IncomeSync.Core.Handlers.AuthHandlers;

public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, TokenResponse>
{
    private readonly IMediator _mediator;

    public CreateAccountHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TokenResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = request.Email,
            Password = request.Password
        };

        var userResponse = await _mediator.Send(createUserRequest, cancellationToken);
        var createLogInRequest = new CreateTokenRequest
        {
            Email = userResponse.Email,
            Password = request.Password
        };

        return await _mediator.Send(createLogInRequest, cancellationToken);
    }
}