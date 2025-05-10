namespace Stefanini.Application.Models.Client;

public record CreateClientCommandInputModel(string Name, DateTime Birthday, string Cpf, string? Email, string? Naturality, string? Nacionality, string? Gender, string? Address);


