using FluentValidation;
using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;

namespace IncomeSync.Api.Validations.UserValidation;

public class CreateLogInRequestValidator : AbstractValidator<CreateLogInRequest>
{
    public CreateLogInRequestValidator()
    {
        RuleFor(_ => _.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(_ => _.Password)
            .NotEmpty();
    }
}