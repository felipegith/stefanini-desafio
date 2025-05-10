using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Models.Client;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Command.Client;

public record CreateClientCommand(CreateClientCommandInputModel Model) : IRequest<ErrorOr<Guid>>;


public sealed class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ErrorOr<Guid>>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateClientCommandHandler(IServiceProvider serviceProvider)
    {
        _clienteRepository = serviceProvider.GetRequiredService<IClienteRepository>();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    }
    public async Task<ErrorOr<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var cpfExists = await _clienteRepository.FindByCpf(request.Model.Cpf);
        if (cpfExists != null)
            return Error.Conflict(code: "Cpf already exists");
        
        try
        {
            var create = Domain.Entities.Client.Create(request.Model.Name, request.Model.Birthday, request.Model.Cpf,
                request.Model.Email, request.Model.Naturality, request.Model.Nacionality, request.Model.Gender,
                request.Model.Address);
            
            _clienteRepository.Create(create);

            await _unitOfWork.Commit();
            return create.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}