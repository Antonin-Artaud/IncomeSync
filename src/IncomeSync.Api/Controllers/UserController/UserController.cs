using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncomeSync.Api.Controllers.UserController;

[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
    {
        var userResponse = await _mediator.Send(createUserRequest);
        return CreatedAtAction(nameof(CreateUser), userResponse);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var getUserRequest = new GetUserRequest
        {
            Id = id
        };

        var userResponse = await _mediator.Send(getUserRequest);

        return userResponse != null ? Ok(userResponse) : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var getUserRequest = new DeleteUserRequest()
        {
            Id = id
        };

        var userResponse = await _mediator.Send(getUserRequest);

        return Ok(userResponse);
    }
}