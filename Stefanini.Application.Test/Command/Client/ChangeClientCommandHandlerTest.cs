using NSubstitute;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Test.Fixture.Client;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Test.Command.Client;

public class ChangeClientCommandHandlerTest
{
    private static readonly ChangeClientCommand Command = new ChangeClientCommand(Guid.NewGuid(), ClientFixture.NameUpdate, String.Empty);
    private readonly ChangeClientCommandHandler _handler;
    private readonly IClienteRepository _clienteRepositoryMoq;
    private readonly IUnitOfWork _unitOfWorkMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    
    public ChangeClientCommandHandlerTest()
    {
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _clienteRepositoryMoq = Substitute.For<IClienteRepository>();
        _unitOfWorkMoq = Substitute.For<IUnitOfWork>();
        _serviceProviderMoq.GetService(typeof(IClienteRepository)).Returns(_clienteRepositoryMoq);
        _serviceProviderMoq.GetService(typeof(IUnitOfWork)).Returns(_unitOfWorkMoq);
        _handler = new ChangeClientCommandHandler(_serviceProviderMoq);
    }


    [Fact]
    public async Task Must_Change_Client()
    {
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).Returns(ClientFixture.ClientMoq);
        _unitOfWorkMoq.Commit().Returns(true);
        var result = await _handler.Handle(Command, CancellationToken.None);
        
        Assert.True(result.Value);
    }
    
    [Fact]
    public async Task Must_Skip_Change_When_Name_IsEmpty()
    {
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).Returns(ClientFixture.ClientMoq);
        var command = new ChangeClientCommand(Guid.NewGuid(), string.Empty, ClientFixture.Gender);
        var result = await _handler.Handle(command, CancellationToken.None);
        
        Assert.True(result.Value);
    }
    
    [Fact]
    public async Task Must_Skip_Change_When_Gender_IsEmpty()
    {
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).Returns(ClientFixture.ClientMoq);
        var command = new ChangeClientCommand(Guid.NewGuid(), ClientFixture.Name, string.Empty);
        var result = await _handler.Handle(command, CancellationToken.None);
        
        Assert.True(result.Value);
    }
}