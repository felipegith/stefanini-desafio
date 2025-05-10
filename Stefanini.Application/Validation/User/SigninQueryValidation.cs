using FluentValidation;
using Stefanini.Application.Query.User;

namespace Stefanini.Application.Validation.User;

public class SigninQueryValidation : AbstractValidator<SigninQuery>
{
    public SigninQueryValidation()
    {
        RuleFor(x=>x.Email).EmailAddress().NotEmpty();
        RuleFor(x=>x.Password).NotEmpty();
    }
}