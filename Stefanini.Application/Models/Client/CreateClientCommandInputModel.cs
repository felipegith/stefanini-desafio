namespace Stefanini.Application.Models.Client;

public record CreateClientCommandInputModel(string Name, DateTime BirthDate, string Cpf, string? Email, string? Naturality, string? Nacionality, string? Gender, string? Address, Guid UserId);


