using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Command.Client;

public record ChangeClientCommand(Guid Id, string Name, string Gender, string Nationality, string Naturality, string Address, string Email) : IRequest<ErrorOr<bool>>;


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
        var errors = new List<Error>();
        var client = await _clienteRepository.FindByIdAsync(request.Id);
        
        if (client == null)
            return Error.NotFound(code: "Client not found");
        
        if(!string.IsNullOrEmpty(request.Name))
            client.ChangeName(request.Name);
        
        if(!string.IsNullOrEmpty(request.Gender))
            client.ChangeGender(request.Gender);

        if (!string.IsNullOrEmpty(request.Email))
        {
            var existsEmail = await _clienteRepository.FindByEmail(request.Email);
            if (existsEmail != null && existsEmail.Id != request.Id)
            {
                errors.Add(Error.Conflict(code: "Client.EmailExists", description: "Email already exists"));
            }
            else
            {
                client.ChangeEmail(request.Email);
            }
        }
            
        
        if(!string.IsNullOrEmpty(request.Naturality))
            client.ChangeNaturality(request.Naturality);
        
        if(!string.IsNullOrEmpty(request.Nationality))
            client.ChangeNacionality(request.Nationality);
        
        if(!string.IsNullOrEmpty(request.Address))
            client.ChangeAddress(request.Address);
        
        await _unitOfWork.Commit();
        if (errors.Any())
            return errors;
        return true;
    }
}
