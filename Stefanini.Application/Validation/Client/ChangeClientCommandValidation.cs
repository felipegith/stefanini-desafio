using FluentValidation;
using Stefanini.Application.Command.Client;

namespace Stefanini.Application.Validation.Client;

public class ChangeClientCommandValidation : AbstractValidator<ChangeClientCommand>
{
    public ChangeClientCommandValidation()
    {
        RuleFor(x=>x.Id).NotEmpty();
    }
}