using MediatR;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;
using Stefanini.Application.Models.User;
using Stefanini.Domain.Interfaces.Repository;
using Stefanini.Domain.Interfaces.Services;

namespace Stefanini.Application.Query.User;

public record SigninQuery(string Email, string Password): IRequest<ErrorOr<SigninOutputModel>>;

public sealed class SigninQueryHandler : IRequestHandler<SigninQuery, ErrorOr<SigninOutputModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly IToken _token;
    public SigninQueryHandler(IServiceProvider serviceProvider)
    {
        _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        _token = serviceProvider.GetRequiredService<IToken>();
    }
    public async Task<ErrorOr<SigninOutputModel>> Handle(SigninQuery request, CancellationToken cancellationToken)
    {
        var findUser = await _userRepository.FindUserAsync(request.Email, request.Password);
        
        if(findUser == null)
            return Error.NotFound(code: "UserNotFound");

        var token = _token.GenerateToken(findUser);

        return new SigninOutputModel(token, findUser.Id);
    }
}


