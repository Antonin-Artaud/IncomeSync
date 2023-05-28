using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IncomeSync.Api.Controllers.AuthController;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> LogIn()
    {
        return Ok();
    }
    
}