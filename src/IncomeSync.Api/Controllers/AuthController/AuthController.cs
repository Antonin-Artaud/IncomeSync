using System.Security.Cryptography;
using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;
using IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.IO.File;

namespace IncomeSync.Api.Controllers.AuthController;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("LogIn")]
    public async Task<IActionResult> LogIn(CreateTokenRequest request)
    {
        var response = await _mediator.Send(request);
        return CreatedAtAction(nameof(LogIn), response);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register(CreateAccountRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet(".well-known/jwks.json")]
    public async Task<IActionResult> GetJwks()
    {
        var publicKeyPath = _configuration["JwtSettings:PublicKeyFilePath"]!;
        var publicKeyBytes = Convert.FromBase64String(await ReadAllTextAsync(publicKeyPath));
    
        var ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
    
        var ecdsaSecurityKey = new ECDsaSecurityKey(ecdsa) { KeyId = "Key1" };

        var webKey = new JsonWebKey
        {
            Kty = "EC",
            Use = "sig",  // signature
            Kid = ecdsaSecurityKey.KeyId,
            X = Base64UrlEncoder.Encode(ecdsaSecurityKey.ECDsa.ExportParameters(false).Q.X),
            Y = Base64UrlEncoder.Encode(ecdsaSecurityKey.ECDsa.ExportParameters(false).Q.Y),
            Crv = "P-256",
        };
    
        return Ok(new JsonWebKeySet(webKey.ToString()));
    }
}