using System.Text.RegularExpressions;
using Stefanini.Domain.Test.Fixture.Client;

namespace Stefanini.Domain.Test.Entities.Client;

public class ClientTest
{
    [Fact]
    public void Must_Create_A_Cliente()
    {
        var client = Domain.Entities.Client.Create(ClientFixture.Name, ClientFixture.BirthDate, ClientFixture.Cpf, ClientFixture.Email, ClientFixture.Naturality, ClientFixture.Nacionality, ClientFixture.Gender, ClientFixture.Address);
        
        Assert.NotNull(client);
        Assert.Equal(client.Name, ClientFixture.Name);
        Assert.Equal(client.BirthDate, ClientFixture.BirthDate);
        Assert.Equal(client.Cpf, ClientFixture.Cpf);
        Assert.IsType<Guid>(client.Id);
        Assert.NotEqual(Guid.Empty, client.Id);
    }

    [Fact]
    public void Dont_Must_Create_A_Client_When_Email_IsInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
            Domain.Entities.Client.Create(
                ClientFixture.Name,
                ClientFixture.BirthDate,
                ClientFixture.Cpf,
                ClientFixture.EmailInvalid,
                ClientFixture.Naturality,
                ClientFixture.Nacionality,
                ClientFixture.Gender,
                ClientFixture.Address
            ));
    }

    [Fact]
    public void Must_Create_A_Client_Without_Options_Field()
    {
        var client = Domain.Entities.Client.Create(ClientFixture.Name, ClientFixture.BirthDate, ClientFixture.Cpf, ClientFixture.Email, ClientFixture.EmptyNaturality, ClientFixture.EmptyNacionality, ClientFixture.EmptyGender, ClientFixture.EmptyAddress);
        
        Assert.NotNull(client);
        Assert.Equal(client.Name, ClientFixture.Name);
        Assert.Equal(client.BirthDate, ClientFixture.BirthDate);
        Assert.Equal(client.Cpf, ClientFixture.Cpf);
        Assert.Equal(client.Address, ClientFixture.EmptyAddress);
        Assert.Equal(client.Gender, ClientFixture.EmptyGender);
        Assert.Equal(client.Naturality, ClientFixture.EmptyNaturality);
        Assert.Equal(client.Nacionality, ClientFixture.EmptyNacionality);
        Assert.IsType<Guid>(client.Id);
        Assert.NotEqual(Guid.Empty, client.Id);
    }

    [Fact]
    public void Dont_Must_Create_A_Client_When_Cpf_IsInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
            Domain.Entities.Client.Create(
                ClientFixture.Name,
                ClientFixture.BirthDate,
                ClientFixture.CpfInvalid,
                ClientFixture.Email,
                ClientFixture.Naturality,
                ClientFixture.Nacionality,
                ClientFixture.Gender,
                ClientFixture.Address
            ));
    }

    [Fact]
    public void Must_Validate_Format_OfThe_Cpf()
    {
        var validate = Regex.IsMatch(ClientFixture.Cpf, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
        
        Assert.True(validate);
    }

    [Fact]
    public void Must_Return_False_When_Cpf_Is_Invalid()
    {
        var validate = Regex.IsMatch(ClientFixture.CpfInvalid, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
        
        Assert.False(validate);
    }

    [Fact]
    public void Must_Validate_If_Email_Is_Valid()
    {
        var validate = Regex.IsMatch(ClientFixture.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        Assert.True(validate);
    }
    
    [Fact]
    public void Must_Return_False_When_Email_Is_Invalid()
    {
        var validate = Regex.IsMatch(ClientFixture.EmailInvalid, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        
        Assert.False(validate);
    }
}