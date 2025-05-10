using NSubstitute;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Test.Fixture.Client;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Test.Command.Client;

public class CreateClientCommandHandlerTest
{
    private static readonly CreateClientCommand Command = new CreateClientCommand(ClientFixture.Model);
    private readonly CreateClientCommandHandler _handler;
    private readonly IClienteRepository _clienteRepositoryMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    public CreateClientCommandHandlerTest()
    {
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _clienteRepositoryMoq = Substitute.For<IClienteRepository>();
        _serviceProviderMoq.GetService(typeof(IClienteRepository)).Returns(_clienteRepositoryMoq);
        _handler = new CreateClientCommandHandler(_serviceProviderMoq);
    }

    [Fact]
    public async Task Must_Return_True_When_Client_Is_Create()
    {
        var result = await _handler.Handle(Command, CancellationToken.None);
        _clienteRepositoryMoq.Received(1).Create(Arg.Any<Domain.Entities.Client>());
        Assert.NotEqual(result.Value, Guid.Empty);
    }
}