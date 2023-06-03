using FluentValidation;
using IncomeSync.Core.Providers.KeysProvider;
using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncomeSync.Api.Controllers.AuthenticationController;

[Route("api/[controller]")]
public class AuthenticationController : SignedResponseController
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateLogInRequest> _createLogInRequestValidator;
    private readonly IValidator<CreateAccountRequest> _createAccountRequestValidator;

    public AuthenticationController(
        IMediator mediator,
        IValidator<CreateLogInRequest> createLogInRequestValidator,
        IValidator<CreateAccountRequest> createAccountRequestValidator,
        IPrivateKeyProvider privateKeyProvider) : base(privateKeyProvider)
    {
        _mediator = mediator;
        _createLogInRequestValidator = createLogInRequestValidator;
        _createAccountRequestValidator = createAccountRequestValidator;
    }

    [AllowAnonymous]
    [HttpPost("LogIn")]
    public async Task<IActionResult> LogIn(CreateLogInRequest request)
    {
        var validationResult = await _createLogInRequestValidator.ValidateAsync(request);

        if (validationResult.IsValid is not true)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var response = await _mediator.Send(request);
        return CreatedAtAction(nameof(LogIn), response);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateAccountRequest request)
    {
        var validationResult = await _createAccountRequestValidator.ValidateAsync(request);

        if (validationResult.IsValid is not true)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _mediator.Send(request);
        return Ok();
    }
}