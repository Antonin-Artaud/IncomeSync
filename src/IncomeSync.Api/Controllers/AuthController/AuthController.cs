using IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncomeSync.Api.Controllers.AuthController;

[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LogIn(CreateTokenRequest request)
    {
        var token = await _mediator.Send(request);
        return Ok(token);
    }
    
}