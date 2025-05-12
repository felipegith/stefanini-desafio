using Microsoft.EntityFrameworkCore;
using Stefanini.Domain.Interfaces.Repository;
using Stefanini.Infrastructure.Context;

namespace Stefanini.Infrastructure.Repositories.Client;

public class ClientRepository : IClienteRepository
{
    private readonly DatabaseContext _context;

    public ClientRepository(DatabaseContext context)
    {
        _context = context;
    }
    public void Create(Domain.Entities.Client cliente)
    {
        try
        {
            _context.Clients.Add(cliente);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool Remove(Domain.Entities.Client client)
    {
        try
        {
            _context.Clients.Remove(client);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Domain.Entities.Client> FindByIdAsync(Guid id)
        => await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Domain.Entities.Client>> FindAllAsync(Guid userId)
        => await _context.Clients.Where(x=>x.UserId == userId).ToListAsync();

    public async Task<Domain.Entities.Client> FindByCpf(string cpf)
        => await _context.Clients.FirstOrDefaultAsync(x => x.Cpf == cpf);
}