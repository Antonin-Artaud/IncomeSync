using System.Net;
using FluentValidation;
using IncomeSync.Core.Shared.Contracts.Responses.ValidationResponse;

namespace IncomeSync.Api.Middlewares.ValidationMiddleware;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _request;

    public ValidationExceptionMiddleware(RequestDelegate request)
    {
        _request = request;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _request(ctx);
        }
        catch (ValidationException exception)
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errorMessageList = exception.Errors.Select(_ => _.ErrorMessage).ToList();

            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = errorMessageList
            };

            await ctx.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}