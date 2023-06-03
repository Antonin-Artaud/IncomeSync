using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using MediatR;

namespace IncomeSync.Core.Handlers.AuthHandlers;

public class CreateAccountHandler : IRequestHandler<CreateAccountRequest>
{
    private readonly IMediator _mediator;

    public CreateAccountHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = request.Email,
            Password = request.Password
        };

        await _mediator.Send(createUserRequest, cancellationToken);
    }
}