using System.Security.Cryptography;
using IncomeSync.Core.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace IncomeSync.Core.Providers.KeysProvider;

public interface IPublicKeyProvider { Task<ECDsa> GetPublicKey(); }

public class PublicKeyProvider : IPublicKeyProvider
{
    private readonly IConfiguration _configuration;

    public PublicKeyProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ECDsa> GetPublicKey()
    {
        var publicKeyPath = _configuration["JwtSettings:PublicKey"] ?? throw new PublicKeyNotFoundException();
        var publicKeyText = await File.ReadAllTextAsync(publicKeyPath);
        
        var publicKey = ECDsa.Create();
        publicKey.ImportFromPem(publicKeyText);

        return publicKey;
    }
}