
using System.Text.RegularExpressions;
using Stefanini.Domain.Test.Fixture.Client;
using Stefanini.Domain.Test.Fixture.User;

namespace Stefanini.Domain.Test.Entities.User;

public class UserTest
{
    [Fact]
    public void Must_Create_A_User()
    {
        var user = Domain.Entities.User.Create(UserFixture.Email, UserFixture.Password);
        
        Assert.NotNull(user);
        Assert.Equal(user.Email, UserFixture.Email);
        Assert.Equal(user.Password, UserFixture.Password);
        
    }
    [Fact]
    public void Must_Validate_If_Email_Is_Valid()
    {
        var validate = Regex.IsMatch(UserFixture.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        Assert.True(validate);
    }
    
    [Fact]
    public void Must_Have_Empty_Client_List_When_Created()
    {
        var user = Domain.Entities.User.Create(UserFixture.Email, UserFixture.Password);
        Assert.NotNull(user.Clients);
        Assert.Empty(user.Clients);
    }
    
    [Fact]
    public void Must_Return_False_When_Email_Is_Invalid()
    {
        var validate = Regex.IsMatch(UserFixture.EmailInvalid, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        
        Assert.False(validate);
    }
}