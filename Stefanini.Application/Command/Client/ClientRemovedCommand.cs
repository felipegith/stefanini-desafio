using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Models.Response;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Command.Client;

public record ClientRemovedCommand(Guid Id) : IRequest<ErrorOr<ResponseOutputModel>>;


public class ClientRemovedCommandHandler : IRequestHandler<ClientRemovedCommand, ErrorOr<ResponseOutputModel>>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientRemovedCommandHandler(IServiceProvider serviceProvider)
    {
        _clienteRepository = serviceProvider.GetRequiredService<IClienteRepository>();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    }
    public async Task<ErrorOr<ResponseOutputModel>> Handle(ClientRemovedCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Error.NotFound(
                code: "Id empty",
                description: "Id cannot be empty"
            );
        
        var client = await _clienteRepository.FindByIdAsync(request.Id);
        if (client == null)
        {
            return Error.NotFound(
                code: "Client not found",
                description: $"Cannot find the client with id {request.Id}"
            );
        }

         _clienteRepository.Remove(client);
         await _unitOfWork.Commit();
        return new ResponseOutputModel(true, "Client removed sucessfully");

    }
}



