using Stefanini.Domain.Entities;

namespace Stefanini.Domain.Interfaces.Services;

public interface IToken
{
    string GenerateToken(Client client);
}