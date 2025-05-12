using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Models.Client;
using Stefanini.Application.Models.Response;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Query.Client;

public record FindClientQuery(Guid Id) : IRequest<ErrorOr<ClientResponseDto>>;


public sealed class FindClientQueryHandler : IRequestHandler<FindClientQuery, ErrorOr<ClientResponseDto>>
{
    private readonly IClienteRepository _clienteRepository;

    public FindClientQueryHandler(IServiceProvider serviceProvider)
    {
        _clienteRepository = serviceProvider.GetRequiredService<IClienteRepository>();
    }
    public async Task<ErrorOr<ClientResponseDto>> Handle(FindClientQuery request, CancellationToken cancellationToken)
    {
        var client = await _clienteRepository.FindByIdAsync(request.Id);

        if (client is null)
            return Error.NotFound(
                code: "Client not found", description: "Client not found");
        
        return new ClientResponseDto(client.Id, client.Name, client.Cpf, client.BirthDate, client.Email, client.Naturality, client.Nacionality, client.Gender, client.Address, client.CreatedAt);
    }
}