using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = "MHcCAQEEIBwUCIgShTYqTbF8h9cW540VE2hiZa9TX9p12YQJektEoAoGCCqGSM49AwEHoUQDQgAErzbLp0Q2jLzgxHAya/M+pyrirLxZB0AbeaU6/k5lVA0XV/2+yakkj0NLjGNNzNbMWfYIsKs38ZTHDPhaeempcw=="u8.ToArray();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", request.UserId)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.EcdsaSha512Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Task.FromResult(new TokenResponse
        {
            Id = token.Id,
            Issuer = token.Issuer,
            SecurityKey = token.SecurityKey,
            SigningKey = token.SigningKey,
            ValidFrom = token.ValidFrom,
            ValidTo = token.ValidTo
        });
    }
}