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
        Assert.Equal(client.CreatedAt.Date, DateTime.Now.Date);
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

    [Fact]
    public void Must_Change_Name()
    {
        var name = "Carlos Alberto";
        var client = new Domain.Entities.Client();
        client.ChangeName(name);
        Assert.Equal(client.UpdatedAt.Value.Date, DateTime.Now.Date);
    }
    
    [Fact]
    public void Must_Change_Gender()
    {
        var gender = "Female";
        var client = new Domain.Entities.Client();
        client.ChangeGender(gender);
        Assert.Equal(client.UpdatedAt.Value.Date, DateTime.Now.Date);
    }

    [Fact]
    public void Must_Change_Naturality()
    {
        var naturality = "São Paulo";
        var client = new Domain.Entities.Client();
        client.ChangeNaturality(naturality);
        Assert.Equal(client.UpdatedAt.Value.Date, DateTime.Now.Date);
    }
    
    [Fact]
    public void Must_Change_Nacionality()
    {
        var nacionality = "São Paulo";
        var client = new Domain.Entities.Client();
        client.ChangeNacionality(nacionality);
        Assert.Equal(client.UpdatedAt.Value.Date, DateTime.Now.Date);
    }
    
    [Fact]
    public void Must_Change_Email()
    {
        var email = ClientFixture.Email;
        var client = new Domain.Entities.Client();
        client.ChangeEmail(email);
        Assert.Equal(client.UpdatedAt.Value.Date, DateTime.Now.Date);
    }
    
    [Fact]
    public void Must_Change_Address()
    {
        var address = "Madureira";
        var client = new Domain.Entities.Client();
        client.ChangeAddress(address);
        Assert.Equal(client.UpdatedAt.Value.Date, DateTime.Now.Date);
    }

    [Fact]
    public void Must_Validate_If_BirthDay_IsValid()
    {
        var birthDay = ClientFixture.BirthDate;
        var result = Domain.Entities.Client.ValidateBirthDate(birthDay);
        Assert.True(result);
    }
    
    [Fact]
    public void Must_Validate_If_BirthDay_Is_DateTimeMinValue()
    {
        var birthDay = DateTime.MinValue;
        var result = Domain.Entities.Client.ValidateBirthDate(birthDay);
        Assert.False(result);
    }
    
    [Fact]
    public void Must_Validate_If_BirthDay_Is_Equal_DateTimeNow()
    {
        var birthDay = DateTime.Now;
        var result = Domain.Entities.Client.ValidateBirthDate(birthDay);
        Assert.False(result);
    }
}