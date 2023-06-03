using IncomeSync.Core.Providers.KeysProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncomeSync.Api.Controllers.ServerController;

public class ServerController : SignedResponseController
{
    private readonly IPublicKeyProvider _publicKeyProvider;
    public ServerController(IPublicKeyProvider publicKeyProvider, IPrivateKeyProvider privateKeyProvider) : base(privateKeyProvider)
    {
        _publicKeyProvider = publicKeyProvider;
    }

    [AllowAnonymous]
    [HttpGet(".well-known/jwks.json")]
    public async Task<IActionResult> GetJwks ()
    {
        var ecdsa = await _publicKeyProvider.GetPublicKey();
        var publicKey = ecdsa.ExportSubjectPublicKeyInfoPem();
        return Ok(publicKey);
    }
}