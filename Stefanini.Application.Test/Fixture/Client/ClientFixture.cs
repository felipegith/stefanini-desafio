using Stefanini.Application.Models.Client;

namespace Stefanini.Application.Test.Fixture.Client;

public static class ClientFixture
{
    public static string Name = "Felipe";
    public static DateTime BirthDate = new DateTime(1996, 12, 24);
    public static string Cpf = "807.929.760-61";
    public static string CpfInvalid = "807.929.760-bx";
    public static string Email = "stefanini@mail.com";
    public static string EmailInvalid = "stefanini.mail.com";
    public static string Gender = "Male";
    public static string Address = "Rio de Janeiro";
    public static string Naturality = "Rio de Janeiro";
    public static string Nacionality = "Brasileira";
    
    public static string NameUpdate = "Carlos Alberto";
    public static CreateClientCommandInputModel Model = new CreateClientCommandInputModel(Name, BirthDate, Cpf, Email, Naturality, Nacionality, Gender, Address);
    
    public static Domain.Entities.Client ClientMoq = Domain.Entities.Client.Create(Name, BirthDate, Cpf, Email, Naturality, Nacionality, Gender, Address);
}