using FluentValidation;
using IncomeSync.Core.Shared.Contracts.Requests.UserRequest;

namespace IncomeSync.Api.Validations.UserValidation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(_ => _.Email).NotEmpty().EmailAddress();
    }
}