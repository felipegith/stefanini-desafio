using System.Text.RegularExpressions;

namespace Stefanini.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string Password { get; private set; }
    public List<Client> Clients { get; private set; }

    public static User Create(string email, string password)
    {
        var user = new User
        {
            Email = !string.IsNullOrEmpty(email) ? ValidateEmail(email) ? email : throw new ArgumentException(): string.Empty,
            Password = password
        };

        return user;
    }
    
    public static bool ValidateEmail(string email)
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
}