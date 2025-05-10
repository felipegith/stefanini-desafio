using NSubstitute;
using Stefanini.Application.Query.Client;
using Stefanini.Application.Query.User;
using Stefanini.Application.Test.Fixture.User;
using Stefanini.Domain.Interfaces.Repository;
using Stefanini.Domain.Interfaces.Services;

namespace Stefanini.Application.Test.Query.User;

public class SigninQueryTest
{
    private static readonly SigninQuery Query = new SigninQuery(UserFixture.Email, UserFixture.Password);
    private readonly SigninQueryHandler _query;
    private readonly IUserRepository _userRepositoryMoq;
    private readonly IServiceProvider _serviceProviderMoq;
    private readonly IToken _tokenMoq;
    
    public SigninQueryTest()
    {
        _serviceProviderMoq = Substitute.For<IServiceProvider>();
        _userRepositoryMoq = Substitute.For<IUserRepository>();
        _tokenMoq = Substitute.For<IToken>();
        _serviceProviderMoq.GetService(typeof(IUserRepository)).Returns(_userRepositoryMoq);
        _serviceProviderMoq.GetService(typeof(IToken)).Returns(_tokenMoq);
        _query = new SigninQueryHandler(_serviceProviderMoq);
    }

    [Fact]
    public async Task Must_Verify_If_Email_And_Password_Are_Correct()
    {
        var userMoq = Domain.Entities.User.Create(UserFixture.Email, UserFixture.Password);
        _userRepositoryMoq.FindUserAsync(UserFixture.Email, UserFixture.Password).Returns(userMoq);
        
        var result = await _query.Handle(Query,  CancellationToken.None);
        _userRepositoryMoq.Received(1).FindUserAsync(UserFixture.Email, UserFixture.Password);
        
    }

    [Fact]
    public async Task Must_Verify_If_Email_And_Password_Are_Correct_And_Return_A_Token()
    {
        var userMoq = Domain.Entities.User.Create(UserFixture.Email, UserFixture.Password);
        _userRepositoryMoq.FindUserAsync(UserFixture.Email, UserFixture.Password).Returns(userMoq);
        _tokenMoq.GenerateToken(userMoq).Returns(UserFixture.Token);
        var result = await _query.Handle(Query,  CancellationToken.None);
        
        Assert.NotNull(result.Value);
        Assert.Contains(".", result.Value);
    }
}