using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Query.Client;
using Stefanini.Application.Test.Fixture.Client;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Test.Query.Client;

public class FindClientQueryHandlerTest
{
    private static readonly FindClientQuery Query = new FindClientQuery(Guid.NewGuid());
    private readonly FindClientQueryHandler _query;
    private readonly IClienteRepository _clienteRepositoryMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    
    public FindClientQueryHandlerTest()
    {
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _clienteRepositoryMoq = Substitute.For<IClienteRepository>();
        _serviceProviderMoq.GetService(typeof(IClienteRepository)).Returns(_clienteRepositoryMoq);
        _query = new FindClientQueryHandler(_serviceProviderMoq);
    }

    [Fact]
    public async Task Must_Verify_If_The_Client_Dont_Exists()
    {
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        var result = await _query.Handle(Query, CancellationToken.None);
        
        Assert.True(result.IsError);
        Assert.Equal(result.FirstError.Code, "Client not found");
    }
    
    [Fact]
    public async Task Must_Return_The_Client_If_Exists_On_Dabase()
    {
        var clientMoq = ClientFixture.ClientMoq;
        _clienteRepositoryMoq.FindByIdAsync(Arg.Any<Guid>()).Returns(clientMoq);
        var result = await _query.Handle(Query, CancellationToken.None);
        
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.Name, clientMoq.Name);
        Assert.Equal(result.Value.Cpf, clientMoq.Cpf);
        Assert.Equal(result.Value.BirthDate, clientMoq.BirthDate);
        
    }
}