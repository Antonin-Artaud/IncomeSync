using System.Text.Json;
using IncomeSync.Core.Shared.Contracts.Responses.UserResponse;
using IncomeSync.Core.Shared.Exceptions.UserExceptions;

namespace IncomeSync.Api.Middlewares.UserExceptionMiddleware;

public class UserExceptionMiddleware
{
    private readonly RequestDelegate _request;

    public UserExceptionMiddleware(RequestDelegate request)
    {
        _request = request;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _request(ctx);
        }
        catch (UserBaseException exception)
        {
            ctx.Response.StatusCode = exception switch
            {
                UserBaseAlreadyExistException => StatusCodes.Status409Conflict,
                UserBaseNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            
            var validationFailureResponse = new UserFailureResponse
            {
                Errors = new []
                {
                    exception.Message
                }
            };

            await ctx.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}