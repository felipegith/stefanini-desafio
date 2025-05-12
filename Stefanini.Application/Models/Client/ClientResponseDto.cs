namespace Stefanini.Application.Models.Client;

public record ClientResponseDto(Guid Id, string Name, string Cpf, DateTime BirthDate, string? Email, string? Naturality, string? Nacionality, string? Gender, string? Address, DateTime CreatedAt);
