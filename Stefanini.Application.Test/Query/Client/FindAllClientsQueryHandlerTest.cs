using NSubstitute;
using Stefanini.Application.Models.Client;
using Stefanini.Application.Query.Client;
using Stefanini.Application.Test.Fixture.Client;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Test.Query.Client;

public class FindAllClientsQueryHandlerTest
{
    private static readonly FindAllClientsQuery Query = new FindAllClientsQuery(ClientFixture.UserId);
    private readonly FindAllClientsQueryHandler _query;
    private readonly IClienteRepository _clienteRepositoryMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    
    public FindAllClientsQueryHandlerTest()
    {
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _clienteRepositoryMoq = Substitute.For<IClienteRepository>();
        _serviceProviderMoq.GetService(typeof(IClienteRepository)).Returns(_clienteRepositoryMoq);
        _query = new FindAllClientsQueryHandler(_serviceProviderMoq);
    }

    [Fact]
    public async Task Must_Return_All_Clients_On_Database()
    {
        var clients = new List<Domain.Entities.Client> { ClientFixture.ClientMoq };
        _clienteRepositoryMoq.FindAllAsync(ClientFixture.UserId).Returns(clients);
        
        var result = await _query.Handle(Query, CancellationToken.None);
        Assert.NotEmpty(result.Value);
        Assert.IsType<List<ClientResponseDto>>(result.Value);
    }
}