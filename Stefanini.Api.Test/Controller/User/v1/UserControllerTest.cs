using Xunit;
using NSubstitute;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stefanini.Api.Controllers.v1;
using Stefanini.Application.Command.User;
using Stefanini.Application.Models.User;
using ErrorOr;
using Stefanini.Application.Test.Fixture.User;

public class UserControllerTest
{
    private readonly IMediator _mediatorMock;
    private readonly UserController _controller;

    public UserControllerTest()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _controller = new UserController(_mediatorMock);
    }

    [Fact]
    public async Task Signup_Should_Return_Created_When_Success()
    {
        var input = new UserInputModel(UserFixture.Email, UserFixture.Password);
        var userId = Guid.NewGuid();

        _mediatorMock.Send(Arg.Any<SignupCommand>(), Arg.Any<CancellationToken>())
            .Returns(ErrorOrFactory.From(userId));

        var result = await _controller.Signup(input, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal($"user/{userId}", createdResult.Location);
        Assert.Equal(userId, createdResult.Value);
    }
    
    [Fact]
    public async Task Signup_Should_Return_BadRequest_When_Validation_Fails()
    {
        var input = new UserInputModel("invalid-email", "");

        var errors = new List<Error>
        {
            Error.Validation("Email", "Invalid email address."),
            Error.Validation("Password", "Password is required")
        };

        _mediatorMock.Send(Arg.Any<SignupCommand>(), Arg.Any<CancellationToken>())
            .Returns((ErrorOr<Guid>)errors);

        var result = await _controller.Signup(input, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Signup_Should_Return_Conflict_When_Email_Already_Exists()
    {
        var input = new UserInputModel(UserFixture.Email, UserFixture.Password);

        var error = Error.Conflict("User.EmailExists", "Email already exists.");

        _mediatorMock.Send(Arg.Any<SignupCommand>(), Arg.Any<CancellationToken>())
            .Returns((ErrorOr<Guid>)new List<Error> { error });

        var result = await _controller.Signup(input, CancellationToken.None);

        Assert.IsType<ConflictObjectResult>(result);
    }
    
    [Fact]
    public async Task Signup_Should_Return_InternalServerError_When_Failure()
    {
        var input = new UserInputModel(UserFixture.Email, UserFixture.Password);

        var error = Error.Failure("User.Unknown", "Internal server error");

        _mediatorMock.Send(Arg.Any<SignupCommand>(), Arg.Any<CancellationToken>())
            .Returns((ErrorOr<Guid>)new List<Error> { error });

        var result = await _controller.Signup(input, CancellationToken.None);

        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
    }

    
        
}
