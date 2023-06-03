using IncomeSync.Core.Services.UserService;
using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;
using IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using IncomeSync.Core.Shared.Entities.UserEntity;
using MediatR;

namespace IncomeSync.Core.Handlers.AuthHandlers;

public class CreateLogInHandler : IRequestHandler<CreateLogInRequest, TokenResponse>
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public CreateLogInHandler(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }


    public async Task<TokenResponse> Handle(CreateLogInRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByCredentialsAsync(request.Email, request.Password);

        var createTokenRequest = new CreateTokenRequest
        {
            User = new UserEntity
            {
                Id = user!.Value.Id,
                Email = user.Value.Email,
            }
        };
        
        var tokenResponse = await _mediator.Send(createTokenRequest, cancellationToken);
        
        return new TokenResponse {Token = tokenResponse.Token};
    }
}