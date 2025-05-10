using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Models.Client;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Query.Client;

public record FindAllClientsQuery : IRequest<ErrorOr<List<ClientResponseDto>>>;


public sealed class FindAllClientsQueryHandler : IRequestHandler<FindAllClientsQuery, ErrorOr<List<ClientResponseDto>>>
{
    private readonly IClienteRepository _clienteRepository;

    public FindAllClientsQueryHandler(IServiceProvider serviceProvider)
    {
        _clienteRepository = serviceProvider.GetRequiredService<IClienteRepository>();
    }
    public async Task<ErrorOr<List<ClientResponseDto>>> Handle(FindAllClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clienteRepository.FindAllAsync();
        
        var response = clients.Select(x=> new ClientResponseDto(x.Name, x.Cpf, x.BirthDate)).ToList();

        return response;

    }
}