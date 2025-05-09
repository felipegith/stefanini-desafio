using System.Text.RegularExpressions;

namespace Stefanini.Domain.Entities;

public class Client : Entity
{
    public string Name { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Cpf { get; private set; }
    public string? Address { get; private set; }
    public string? Gender { get; private set; }
    public string? Email { get; private set; }
    public string? Naturality { get; private set; }
    public string? Nacionality { get; private set; }

    public static Client Create(string name, DateTime birthDate, string cpf, string? email, string ? naturality, string? nacionality, string? gender, string? address)
    {
        var client = new Client
        {
            Name = name,
            BirthDate = birthDate,
            Cpf = ValidateCpf(cpf) ? cpf : throw new ArgumentException(),
            Email = !string.IsNullOrEmpty(email) ? ValidateEmail(email) ? email : throw new ArgumentException(): string.Empty,
            Naturality = naturality,
            Nacionality = nacionality,
            Address = address,
            Gender = gender,
        };
        return client;
    }

    public static bool ValidateCpf(string cpf)
        =>Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
    
    public static bool ValidateEmail(string email)
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    
}