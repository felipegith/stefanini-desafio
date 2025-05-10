using Stefanini.Domain.Entities;

namespace Stefanini.Domain.Interfaces.Repository;

public interface IUserRepository
{
    void Create(User user);
    
    Task<User> FindByEmailAsync(string email);
}