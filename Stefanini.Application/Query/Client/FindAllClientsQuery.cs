using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Models.Client;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Query.Client;

public record FindAllClientsQuery(Guid UserId) : IRequest<ErrorOr<List<ClientResponseDto>>>;


public sealed class FindAllClientsQueryHandler : IRequestHandler<FindAllClientsQuery, ErrorOr<List<ClientResponseDto>>>
{
    private readonly IClienteRepository _clienteRepository;

    public FindAllClientsQueryHandler(IServiceProvider serviceProvider)
    {
        _clienteRepository = serviceProvider.GetRequiredService<IClienteRepository>();
    }
    public async Task<ErrorOr<List<ClientResponseDto>>> Handle(FindAllClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clienteRepository.FindAllAsync(request.UserId);
        
        if(!clients.Any())
            return Error.NotFound("No clients found");

        var response = clients.Select(x=> new ClientResponseDto(x.Id, x.Name, x.Cpf, x.BirthDate, x.Email, x.Naturality, x.Nacionality, x.Gender, x.Address, x.CreatedAt)).ToList();

        return response;

    }
}