using FluentValidation;
using Stefanini.Application.Command.Client;

namespace Stefanini.Application.Validation.Client;

public class ClientRemovedCommandValidation : AbstractValidator<ClientRemovedCommand>
{
    public ClientRemovedCommandValidation()
    {
        RuleFor(x=>x.Id).NotEmpty();
    }
}