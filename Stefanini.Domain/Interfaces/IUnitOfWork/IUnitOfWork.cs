namespace Stefanini.Domain.Interfaces.IUnitOfWork;

public interface IUnitOfWork
{
    Task<bool> Commit();
}