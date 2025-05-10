using ErrorOr;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Stefanini.Application.Command.User;
using Stefanini.Application.Test.Fixture.Client;
using Stefanini.Application.Test.Fixture.User;
using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Domain.Interfaces.Repository;

namespace Stefanini.Application.Test.Command.User;

public class SignupCommandTest
{
    private static readonly SignupCommand Command = new SignupCommand(UserFixture.Email, UserFixture.Password);
    private readonly SignupCommandHandler _handler;
    private readonly IUserRepository _userRepositoryMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    private readonly IUnitOfWork _unitOfWorkMoq;
    public SignupCommandTest()
    {
        _unitOfWorkMoq = Substitute.For<IUnitOfWork>();
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _userRepositoryMoq = Substitute.For<IUserRepository>();
        _serviceProviderMoq.GetService(typeof(IUserRepository)).Returns(_userRepositoryMoq);
        _serviceProviderMoq.GetService(typeof(IUnitOfWork)).Returns(_unitOfWorkMoq);
        _handler = new SignupCommandHandler(_serviceProviderMoq);
    }

    [Fact]
    public async Task Must_Verify_If_Email_Already_Exists()
    {
        var userMoq = Domain.Entities.User.Create(UserFixture.Email, UserFixture.Password);
        _userRepositoryMoq.FindByEmailAsync(Arg.Any<string>()).Returns(userMoq);
        
        var result = await _handler.Handle(Command, CancellationToken.None);
        
        Assert.True(result.IsError);
        Assert.Equal(result.FirstError.Type, ErrorType.Conflict);
    }

    [Fact]
    public async Task Must_Create_User_When_Email_Dont_Exists()
    {
        _unitOfWorkMoq.Commit().Returns(true);
        var userMoq = Domain.Entities.User.Create(UserFixture.Email, UserFixture.Password);
        _userRepositoryMoq.FindByEmailAsync(Arg.Any<string>()).ReturnsNull();
        var result = await _handler.Handle(Command, CancellationToken.None);
        
        _userRepositoryMoq.Received(1).Create(Arg.Any<Domain.Entities.User>());
        Assert.False(result.IsError);
        Assert.NotEqual(result.Value, Guid.Empty);
        
    }
}