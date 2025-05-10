using Microsoft.EntityFrameworkCore;
using Stefanini.Domain.Interfaces.Repository;
using Stefanini.Infrastructure.Context;

namespace Stefanini.Infrastructure.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }
    public void Create(Domain.Entities.User user)
    {
        try
        {
            _context.Users.Add(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Domain.Entities.User> FindByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(x=>x.Email == email);

    public async Task<Domain.Entities.User> FindUserAsync(string email, string password)
        => await _context.Users.FirstOrDefaultAsync(x=>x.Email == email && x.Password == password);
}