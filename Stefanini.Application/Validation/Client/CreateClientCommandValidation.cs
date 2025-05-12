using FluentValidation;
using Stefanini.Application.Command.Client;


namespace Stefanini.Application.Validation.Client;

public class CreateClientCommandValidation : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidation()
    {
        RuleFor(x=>x.Model.Name).NotEmpty();
        RuleFor(x => x.Model.Cpf).NotEmpty();
        RuleFor(x=>x.Model.BirthDate).LessThanOrEqualTo(DateTime.Today.AddYears(-18)).
            WithMessage("You must have age greater than 18.").NotEmpty().WithMessage("Birthday is required.");
        
    }
}