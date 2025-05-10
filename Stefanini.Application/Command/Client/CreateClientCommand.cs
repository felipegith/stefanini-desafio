using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Models.Client;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Command.Client;

public record CreateClientCommand(CreateClientCommandInputModel Model) : IRequest<ErrorOr<Guid>>;


public sealed class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ErrorOr<Guid>>
{
    private readonly IClienteRepository _clienteRepository;

    public CreateClientCommandHandler(IServiceProvider serviceProvider)
    {
        _clienteRepository = serviceProvider.GetRequiredService<IClienteRepository>();
    }
    public async Task<ErrorOr<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var create = Domain.Entities.Client.Create(request.Model.Name, request.Model.Birthday, request.Model.Cpf,
                request.Model.Email, request.Model.Naturality, request.Model.Nacionality, request.Model.Gender,
                request.Model.Address);
            
            _clienteRepository.Create(create);

            return create.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}