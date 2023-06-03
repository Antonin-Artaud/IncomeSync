using FluentValidation;
using IncomeSync.Core.Shared.Contracts.Requests.AuthRequest;

namespace IncomeSync.Api.Validations.UserValidation;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(_ => _.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(_ => _.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain one or more uppercase characters.")
            .Matches("[a-z]").WithMessage("Password must contain one or more lowercase characters.")
            .Matches("[0-9]").WithMessage("Password must contain one or more digit characters.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain one or more special characters.");;
    }
}