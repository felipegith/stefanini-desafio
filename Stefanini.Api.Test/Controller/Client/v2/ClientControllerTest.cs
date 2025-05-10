using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Stefanini.Api.Controllers.v2;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Models.Client;
using Stefanini.Application.Query.Client;
using Stefanini.Application.Test.Fixture.Client;

namespace Stefanini.Api.Test.Controller.Client.v2;

public class ClientControllerTest
{
    private readonly IMediator _mediatorMock;
    private readonly ClientController _controller;

    public ClientControllerTest()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _controller = new ClientController(_mediatorMock);
    }
    
    [Fact]
    public async Task Create_Should_Return_Created_When_Success()
    {
        var input = new CreateClientCommandInputModel(ClientFixture.Name, ClientFixture.BirthDate, ClientFixture.Cpf, ClientFixture.Email, ClientFixture.Naturality, ClientFixture.Nacionality, ClientFixture.Gender, ClientFixture.Address);

        _mediatorMock.Send(Arg.Any<CreateClientCommand>(), Arg.Any<CancellationToken>())
            .Returns(ErrorOrFactory.From(Guid.NewGuid()));

        var result = await _controller.Create(input, CancellationToken.None);

        Assert.IsType<CreatedResult>(result);
    }
    
    [Fact]
    public async Task Create_Should_Return_BadRequest_When_ValidationFails()
    {
        var input = new CreateClientCommandInputModel(string.Empty, DateTime.MinValue, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Empty);

        var errors = new List<Error> { Error.Validation("Name", "Invalid name") };

        _mediatorMock.Send(Arg.Any<CreateClientCommand>(), Arg.Any<CancellationToken>())
            .Returns((ErrorOr<Guid>)errors);

        var result = await _controller.Create(input, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Clients_Should_Return_Ok()
    {
        _mediatorMock.Send(Arg.Any<FindAllClientsQuery>(), Arg.Any<CancellationToken>())
            .Returns(ErrorOrFactory.From(new List<ClientResponseDto>()));

        var result = await _controller.Clients(CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Client_Should_Return_NotFound_If_NotExists()
    {
        _mediatorMock.Send(Arg.Any<FindClientQuery>(), Arg.Any<CancellationToken>())
            .Returns((ErrorOr<ClientResponseDto>)Error.NotFound("Client.NotFound", "Client not found"));

        var result = await _controller.Client(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public async Task Delete_Should_Return_NoContent_When_Success()
    {
        _mediatorMock.Send(Arg.Any<ClientRemovedCommand>(), Arg.Any<CancellationToken>())
            .Returns(ErrorOrFactory.From(new Stefanini.Application.Models.Response.ResponseOutputModel(true, "Client removed sucessfully")));

        var result = await _controller.Delete(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task Update_Should_Return_NoContent_When_Success()
    {
        var input = new ChangeClientInputModel(Guid.NewGuid(), "Carlos Alberto", "M");
        
        _mediatorMock.Send(Arg.Any<ChangeClientCommand>(), Arg.Any<CancellationToken>())
            .Returns(ErrorOrFactory.From(true));

        var result = await _controller.Update(input, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }
}