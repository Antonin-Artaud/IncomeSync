using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IncomeSync.Core.Shared.Contracts.Requests.TokenRequest;
using IncomeSync.Core.Shared.Contracts.Responses.TokenResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IncomeSync.Core.Handlers.TokenHandler;

public class CreateTokenHandler : IRequestHandler<CreateTokenRequest, TokenResponse>
{
    private readonly IConfiguration _configuration;

    public CreateTokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<TokenResponse> Handle(CreateTokenRequest request, CancellationToken cancellationToken)
    {
        var issuer = _configuration["JwtSettings:Issuer"] ?? throw new NullReferenceException("Unable de read Issuer property");
        var audience = _configuration["JwtSettings:Audience"] ?? throw new NullReferenceException("Unable de read Audience property");
        var key = _configuration["JwtSettings:PrivateKey"] ?? throw new NullReferenceException("Unable de read PrivateKey property");
        var privateKeyBytes = Convert.FromBase64String(key);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
        var signingCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha512);

        var subject = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, "teste"),
            new Claim(JwtRegisteredClaimNames.Email, request.Email)
        });

        var expires = DateTime.UtcNow.AddDays(1);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Task.FromResult(new TokenResponse
        {
            Token = jwtToken
        });
    }
}