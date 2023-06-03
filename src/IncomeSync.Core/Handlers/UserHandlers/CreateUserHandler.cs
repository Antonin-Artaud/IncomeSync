using IncomeSync.Core.Services.UserService;
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using MediatR;

namespace IncomeSync.Core.Handlers.UserHandlers;

public class CreateUserHandler : IRequestHandler<CreateUserRequest>
{
    private readonly IUserService _userService;

    public CreateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        await _userService.CreateUserAsync(request);
    }
}