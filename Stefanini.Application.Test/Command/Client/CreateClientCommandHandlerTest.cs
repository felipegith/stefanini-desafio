using ErrorOr;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Test.Fixture.Client;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Test.Command.Client;

public class CreateClientCommandHandlerTest
{
    private static readonly CreateClientCommand Command = new CreateClientCommand(ClientFixture.Model);
    private readonly CreateClientCommandHandler _handler;
    private readonly IClienteRepository _clienteRepositoryMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    private readonly IUnitOfWork _unitOfWorkMoq;
    public CreateClientCommandHandlerTest()
    {
        _unitOfWorkMoq = Substitute.For<IUnitOfWork>();
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _clienteRepositoryMoq = Substitute.For<IClienteRepository>();
        _serviceProviderMoq.GetService(typeof(IClienteRepository)).Returns(_clienteRepositoryMoq);
        _serviceProviderMoq.GetService(typeof(IUnitOfWork)).Returns(_unitOfWorkMoq);
        _handler = new CreateClientCommandHandler(_serviceProviderMoq);
    }

    [Fact]
    public async Task Must_Return_True_When_Client_Is_Create()
    {
        _unitOfWorkMoq.Commit().Returns(true);
        _clienteRepositoryMoq.FindByCpf(Arg.Any<string>()).ReturnsNull();
        var result = await _handler.Handle(Command, CancellationToken.None);
        _clienteRepositoryMoq.Received(1).Create(Arg.Any<Domain.Entities.Client>());
        Assert.NotEqual(result.Value, Guid.Empty);
    }
    
    [Fact]
    public async Task Dont_Must_Create_The_Client_When_Cpf_Already_Exists()
    {
        _unitOfWorkMoq.Commit().Returns(true);
        _clienteRepositoryMoq.FindByCpf(Arg.Any<string>()).Returns(ClientFixture.ClientMoq);
        var result = await _handler.Handle(Command, CancellationToken.None);
        _clienteRepositoryMoq.Received(1).FindByCpf(Arg.Any<string>());
        Assert.True(result.IsError);
        Assert.Equal(result.FirstError.Type, ErrorType.Conflict);
    }
}