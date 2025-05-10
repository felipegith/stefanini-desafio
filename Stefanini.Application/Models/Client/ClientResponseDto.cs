namespace Stefanini.Application.Models.Client;

public record ClientResponseDto(string Name, string Cpf, DateTime BirthDate, string? Email, string? Naturality, string? Nacionality, string? Gender, string? Address, DateTime CreatedAt);
