using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IncomeSync.Api.Middlewares.JwtTokenMiddleware;

public class JwtTokenMiddleware
{
    private readonly RequestDelegate _request;
    private readonly SymmetricSecurityKey _securityKey;
    private readonly IConfiguration _configuration;

    public JwtTokenMiddleware(RequestDelegate request, IConfiguration configuration)
    {
        _request = request;
        _configuration = configuration;
        _securityKey = new SymmetricSecurityKey("MHcCAQEEIBwUCIgShTYqTbF8h9cW540VE2hiZa9TX9p12YQJektEoAoGCCqGSM49AwEHoUQDQgAErzbLp0Q2jLzgxHAya/M+pyrirLxZB0AbeaU6/k5lVA0XV/2+yakkj0NLjGNNzNbMWfYIsKs38ZTHDPhaeempcw=="u8.ToArray());
    }
    
    public async Task Invoke(HttpContext context)
    {
        var AuthorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        var token = AuthorizationHeader?.Split(" ").Last();

        if (token != null)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://incomesync.fr",
                    ValidateAudience = true,
                    ValidAudience = "https://incomesync.fr",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey("MHcCAQEEIBwUCIgShTYqTbF8h9cW540VE2hiZa9TX9p12YQJektEoAoGCCqGSM49AwEHoUQDQgAErzbLp0Q2jLzgxHAya/M+pyrirLxZB0AbeaU6/k5lVA0XV/2+yakkj0NLjGNNzNbMWfYIsKs38ZTHDPhaeempcw=="u8.ToArray()),
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                }, out SecurityToken validatedToken);
                
                // Si la validation est réussie, vous pouvez associer le principal à l'utilisateur du contexte HTTP.
                context.User = principal;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                // Handle the exception according to your requirements.
                // You could log it, return a specific HTTP status code, etc.
            }
        }

        await _request(context);
    }
}