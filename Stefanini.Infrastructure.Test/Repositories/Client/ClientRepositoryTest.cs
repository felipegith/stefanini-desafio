using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;
using Stefanini.Domain.Test.Fixture.Client;
using Stefanini.Infrastructure.Context;
using Stefanini.Infrastructure.Repositories.Client;

namespace Stefanini.Infrastructure.Test.Repositories.Client;

public class ClientRepositoryTest
{
    private readonly ClientRepository _clientRepositoryMoq;
    private readonly UnitOfWork.UnitOfWork _unitOfWorkMoq;
    private readonly DatabaseContext _context;
    private readonly Domain.Entities.Client _clientMoq;
    public ClientRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "DocumentTestDb")
            .Options;
        
        _context = new DatabaseContext(options);
        _clientRepositoryMoq = new Infrastructure.Repositories.Client.ClientRepository(_context);
        _unitOfWorkMoq = new UnitOfWork.UnitOfWork(_context);
        _clientMoq = Domain.Entities.Client.Create(ClientFixture.Name, ClientFixture.BirthDate, ClientFixture.Cpf, ClientFixture.Email, ClientFixture.EmptyNaturality, ClientFixture.EmptyNacionality, ClientFixture.EmptyGender, ClientFixture.EmptyAddress, ClientFixture.UserId);
    }
    [Fact]
    public async Task Must_Create_An_Client_On_Database()
    {
        _clientRepositoryMoq.Create(_clientMoq);
        await _unitOfWorkMoq.Commit();
        
        var find = await _clientRepositoryMoq.FindByIdAsync(_clientMoq.Id);
        
        Assert.NotNull(find);
        
        Assert.Equal(_clientMoq.Id, find.Id);
        Assert.Equal(_clientMoq.Name, find.Name);
        Assert.Equal(_clientMoq.BirthDate, find.BirthDate);
        Assert.Equal(_clientMoq.Cpf, find.Cpf);
    }

    [Fact]
    public void Must_Throw_Exception_When_Create_Client()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new DatabaseContext(options);
        var repository = new ClientRepository(context);

        context.Dispose();

        var client = Domain.Entities.Client.Create(
            ClientFixture.Name,
            ClientFixture.BirthDate,
            ClientFixture.Cpf,
            ClientFixture.Email,
            ClientFixture.EmptyNaturality,
            ClientFixture.EmptyNacionality,
            ClientFixture.EmptyGender,
            ClientFixture.EmptyAddress,
            ClientFixture.UserId
        );

        var ex = Assert.Throws<ObjectDisposedException>(() => repository.Create(client));
        Assert.Contains("DatabaseContext", ex.ObjectName);
    }

    [Fact]
    public async Task Must_Throw_Exception_When_SaveChanges_Fails()
    {
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        unitOfWorkMock.Commit().Throws(new Exception("An error ocurred"));

        var contextOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new DatabaseContext(contextOptions);
        var repository = new ClientRepository(context);

        var client = Domain.Entities.Client.Create(
            ClientFixture.Name,
            ClientFixture.BirthDate,
            ClientFixture.Cpf,
            ClientFixture.Email,
            ClientFixture.EmptyNaturality,
            ClientFixture.EmptyNacionality,
            ClientFixture.EmptyGender,
            ClientFixture.EmptyAddress,
            ClientFixture.UserId
        );

        repository.Create(client);

        
        var exception = await Assert.ThrowsAsync<Exception>(() => unitOfWorkMock.Commit());
        Assert.Equal("An error ocurred", exception.Message);
    }

    [Fact]
    public async Task Must_Remove_A_Client_On_Database()
    {
        await Must_Create_An_Client_On_Database();
        var clientExists = await _clientRepositoryMoq.FindByIdAsync(_clientMoq.Id);
        
        _clientRepositoryMoq.Remove(clientExists);
        _unitOfWorkMoq.Commit();
        
        var clientDontExists = await _clientRepositoryMoq.FindByIdAsync(_clientMoq.Id);
        Assert.NotNull(clientExists);
        Assert.Null(clientDontExists);
        
    }

    [Fact]
    public async Task Must_Find_Client_By_Id()
    {
        await Must_Create_An_Client_On_Database();
        var find = await _clientRepositoryMoq.FindByIdAsync(_clientMoq.Id);
        
        Assert.NotNull(find);
        Assert.Equal(_clientMoq.Id, find.Id);
    }
    
    [Fact]
    public async Task Must_Find_Client_By_Email()
    {
        await Must_Create_An_Client_On_Database();
        var find = await _clientRepositoryMoq.FindByEmail(_clientMoq.Email);
        
        Assert.NotNull(find);
        Assert.Equal(_clientMoq.Email, find.Email);
    }

    [Fact]
    public async Task Must_Return_All_Clients_On_Database()
    {
        await Must_Create_An_Client_On_Database();
        var clients = await _clientRepositoryMoq.FindAllAsync(ClientFixture.UserId);
        
        Assert.NotEmpty(clients);
        Assert.IsType<List<Domain.Entities.Client>>(clients);
    }
    
    [Fact]
    public void Must_Throw_Exception_On_Remove_When_Client_Is_Null()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new DatabaseContext(options);
        var repository = new ClientRepository(context);

        var exception = Assert.Throws<ArgumentNullException>(() => repository.Remove(null));
        Assert.Contains("Value cannot be null", exception.Message);
    }

    [Fact]
    public async Task Must_Found_Same_Cpf_On_Database()
    {
        _clientRepositoryMoq.Create(ClientFixture.ClientMoq);
        await _unitOfWorkMoq.Commit();
        var find = await _clientRepositoryMoq.FindByCpf(_clientMoq.Cpf);
        
        Assert.NotNull(find);
        Assert.Equal(_clientMoq.Cpf, find.Cpf);
    }

    [Fact]
    public async Task Dont_Must_Found_Same_Cpf_On_Database()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        await using var context = new DatabaseContext(options);
        var repository = new ClientRepository(context);

        var result = await repository.FindByCpf(ClientFixture.Cpf);

        Assert.Null(result);
    }

}