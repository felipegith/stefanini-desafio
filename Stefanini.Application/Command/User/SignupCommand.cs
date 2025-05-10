using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Command.Client;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Command.User;

public record SignupCommand(string Email, string Password) : IRequest<ErrorOr<Guid>>;

public sealed class SignupCommandHandler : IRequestHandler<SignupCommand, ErrorOr<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public SignupCommandHandler(IServiceProvider serviceProvider)
    {
        _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    }
    public async Task<ErrorOr<Guid>> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        if(await _userRepository.FindByEmailAsync(request.Email) is not null)
            return Error.Conflict(code: "Email already exists");
        
        var user = Domain.Entities.User.Create(request.Email, request.Password);
        
        _userRepository.Create(user);
        await _unitOfWork.Commit();
        return user.Id;
    }
}