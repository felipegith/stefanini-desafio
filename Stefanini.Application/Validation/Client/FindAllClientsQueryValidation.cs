using FluentValidation;
using Stefanini.Application.Query.Client;

namespace Stefanini.Application.Validation.Client;

public class FindAllClientsQueryValidation : AbstractValidator<FindAllClientsQuery>
{
    public FindAllClientsQueryValidation()
    {
        
    }
}