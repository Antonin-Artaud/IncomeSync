using IncomeSync.Core.Services.UserService;
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using MediatR;

namespace IncomeSync.Core.Handlers.UserHandler;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, UserResponse>
{
    private readonly IUserService _userService;

    public CreateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        return await _userService.CreateUserAsync(request);
    }
}