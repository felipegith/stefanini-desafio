using System.Runtime.InteropServices.JavaScript;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Test.Fixture.Client;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Test.Command.Client;

public class ClientRemovedComandHandlerTest
{
    private static readonly ClientRemovedCommand Command = new ClientRemovedCommand(Guid.NewGuid());
    private readonly ClientRemovedCommandHandler _handler;
    private readonly IClienteRepository _clienteRepositoryMoq;
    private readonly IUnitOfWork _unitOfWorkMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    
    public ClientRemovedComandHandlerTest()
    {
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _clienteRepositoryMoq = Substitute.For<IClienteRepository>();
        _unitOfWorkMoq = Substitute.For<IUnitOfWork>();
        _serviceProviderMoq.GetService(typeof(IClienteRepository)).Returns(_clienteRepositoryMoq);
        _serviceProviderMoq.GetService(typeof(IUnitOfWork)).Returns(_unitOfWorkMoq);
        _handler = new ClientRemovedCommandHandler(_serviceProviderMoq);
    }

    [Fact]
    public async Task Must_Verify_If_Id_Is_Now_Empty()
    {
        ClientRemovedCommand command = new ClientRemovedCommand(Guid.Empty);
        
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result.IsError);
        Assert.Equal(result.FirstError.Code, "Id empty");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Must_Find_The_Client_Before_OfThe_Delete()
    {
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).Returns(ClientFixture.ClientMoq);
        var result  = await _handler.Handle(Command, CancellationToken.None);
        Assert.False(result.IsError);
    }
    
    [Fact]
    public async Task Client_NotFound_To_Remove()
    {
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        var result  = await _handler.Handle(Command, CancellationToken.None);
        Assert.True(result.IsError);
        Assert.Equal(result.FirstError.Code, "Client not found");
    }

    [Fact]
    public async Task Must_Remove_The_Client_With_Sucessfully()
    {
        _unitOfWorkMoq.Commit().Returns(true);
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).Returns(ClientFixture.ClientMoq);
        var result =  await _handler.Handle(Command, CancellationToken.None);
        
        _unitOfWorkMoq.Received(1);
        Assert.True(result.Value.Status);
    }
}