using FluentValidation;
using Stefanini.Application.Query.Client;

namespace Stefanini.Application.Validation.Client;

public class FindClientQueryValidation : AbstractValidator<FindClientQuery>
{
    public FindClientQueryValidation()
    {
        
    }
}