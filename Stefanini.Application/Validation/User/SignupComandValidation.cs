using FluentValidation;
using Stefanini.Application.Command.User;
using Stefanini.Application.Models.User;

namespace Stefanini.Application.Validation.User;

public class SignupComandValidation : AbstractValidator<SignupCommand>
{
    public SignupComandValidation()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }
}