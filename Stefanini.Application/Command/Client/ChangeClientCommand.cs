using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Command.Client;

public record ChangeClientCommand(Guid Id, string Name, string Gender) : IRequest<ErrorOr<bool>>;


public sealed class ChangeClientCommandHandler : IRequestHandler<ChangeClientCommand, ErrorOr<bool>>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeClientCommandHandler(IServiceProvider serviceProvider)
    {
        _clienteRepository = serviceProvider.GetRequiredService<IClienteRepository>();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    }
    public async Task<ErrorOr<bool>> Handle(ChangeClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clienteRepository.FindByIdAsync(request.Id);
        
        if (client == null)
            return Error.NotFound(code: "Client not found");
        
        if(!string.IsNullOrEmpty(request.Name))
            client.ChangeName(request.Name);
        
        if(!string.IsNullOrEmpty(request.Gender))
            client.ChangeGender(request.Gender);
        
        await _unitOfWork.Commit();

        return true;
    }
}
