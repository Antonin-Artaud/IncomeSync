using System.Security.Cryptography;
using IncomeSync.Core.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace IncomeSync.Core.Providers.KeysProvider;

public interface IPrivateKeyProvider { Task<ECDsa> GetPrivateKey(); }

public class PrivateKeyProvider : IPrivateKeyProvider
{
    private readonly IConfiguration _configuration;

    public PrivateKeyProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ECDsa> GetPrivateKey()
    {
        var privateKeyPath = _configuration["JwtSettings:PrivateKey"] ?? throw new PrivateKeyNotFoundException();
        var privateKeyText = await File.ReadAllTextAsync(privateKeyPath);
        
        var privateKey = ECDsa.Create();
        privateKey.ImportFromPem(privateKeyText);

        return privateKey;
    }
}
