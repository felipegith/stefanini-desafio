namespace Stefanini.Domain.Test.Fixture.Client;

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
    public static Guid UserId = Guid.NewGuid();
    
    public static string EmptyGender = string.Empty;
    public static string EmptyAddress = string.Empty;
    public static string EmptyNaturality = string.Empty;
    public static string EmptyNacionality = string.Empty;

    public static string SecretMoq = "9f4c1d82a7be431f91d0e6c3f53a2b19";
    public static string AudienceMoq = "AudienceMoq";
    public static string IssuerMoq = "IssuerMoq";
    
    public static Domain.Entities.Client ClientMoq = Domain.Entities.Client.Create(Name, BirthDate, Cpf, Email, Naturality, Nacionality, Gender, Address, UserId);
    
}