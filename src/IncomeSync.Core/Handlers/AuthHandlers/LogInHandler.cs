using IncomeSync.Core.Services.UserService;
using IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using MediatR;

namespace IncomeSync.Core.Handlers.AuthHandlers;

public class LogInHandler : IRequestHandler<CreateTokenRequest, TokenResponse>
{
    private readonly IUserService _userService;

    public LogInHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<TokenResponse> Handle(CreateTokenRequest request, CancellationToken cancellationToken)
    {
        var userResponse = await _userService.GetUserByEmailAsync(request.Email);

        return new TokenResponse();
    }
}