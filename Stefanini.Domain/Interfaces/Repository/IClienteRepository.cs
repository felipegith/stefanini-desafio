using Stefanini.Domain.Entities;

namespace Stefanini.Domain.Interfaces.Repository;

public interface IClienteRepository
{
    void Create(Client cliente);
    bool Remove(Client client);
    Task<Client> FindByIdAsync(Guid id);
    Task<List<Client>> FindAllAsync();
    Task<Client> FindByCpf(string cpf);
}