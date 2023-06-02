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
    private readonly IMediator _mediator;

    private const string IssuerKey = "JwtSettings:Issuer";
    private const string AudienceKey = "JwtSettings:Audience";
    private const string PrivateKeyFilePathKey = "JwtSettings:PrivateKeyFilePath";

    public CreateTokenHandler(IConfiguration configuration, IMediator mediator)
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    public async Task<TokenResponse> Handle(CreateTokenRequest request, CancellationToken cancellationToken)
    {
        var issuer = GetConfigurationValue(IssuerKey);
        var audience = GetConfigurationValue(AudienceKey);
        var privateKey = await ReadFileAsync(GetConfigurationValue(PrivateKeyFilePathKey));

        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(privateKey);
        var signingCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha512);

        var subject = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, request.User.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, request.User.Email)
        });

        var expires = DateTime.UtcNow.AddDays(1);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Expires = expires,
            Issuer = issuer,
            IssuedAt = DateTime.UtcNow,
            Audience = audience,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return new TokenResponse
        {
            Token = jwtToken
        };
    }

    private string GetConfigurationValue(string key)
    {
        return _configuration[key] ?? throw new NullReferenceException($"Unable de read {key} property");
    }

    private static async Task<string> ReadFileAsync(string path)
    {
        try
        {
            using var reader = File.OpenText(path);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to read file at path: {path}", ex);
        }
    }
}
