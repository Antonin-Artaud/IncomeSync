using IncomeSync.Core.Services.UserService;
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using MediatR;

namespace IncomeSync.Core.Handlers.UserHandlers;

public class GetUserHandler : IRequestHandler<GetUserRequest, UserResponse?>
{
    private readonly IUserService _userService;

    public GetUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserResponse?> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        return await _userService.GetUserByIdAsync(request.Id);
    }
}