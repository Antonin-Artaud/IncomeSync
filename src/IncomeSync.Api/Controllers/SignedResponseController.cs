using System.Security.Cryptography;
using System.Text;
using IncomeSync.Core.Providers.KeysProvider;
using IncomeSync.Core.Shared.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IncomeSync.Api.Controllers;

public class SignedResponseController : ControllerBase
{
    private readonly IPrivateKeyProvider _privateKeyProvider;

    public SignedResponseController(IPrivateKeyProvider privateKeyProvider)
    {
        _privateKeyProvider = privateKeyProvider;
    }

    private static string SignData(string value, ECDsa privateKey)
    {
        var data = Encoding.UTF8.GetBytes(value).AsSpan();
        var signature = privateKey.SignData(data, HashAlgorithmName.SHA512).AsSpan();
        return Convert.ToBase64String(signature);
    }
     
    public override OkObjectResult Ok(object? value)
    {
        var serializedData = JsonConvert.SerializeObject(value);
        var signature = SignData(serializedData, _privateKeyProvider.GetPrivateKey().Result);
        
        return base.Ok(new SignedObjectResponse
        {
            Data = serializedData,
            Signature = signature
        });
    }
    
    public override CreatedAtActionResult CreatedAtAction(string? actionName, object? value)
    {
        var serializedData = JsonConvert.SerializeObject(value);
        var signature = SignData(serializedData, _privateKeyProvider.GetPrivateKey().Result);
        return base.CreatedAtAction(actionName, new SignedObjectResponse
        {
            Data = serializedData,
            Signature = signature
        });
    }
}