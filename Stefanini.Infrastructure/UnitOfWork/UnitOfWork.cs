using Stefanini.Domain.Interfaces.IUnitOfWork;
using Stefanini.Infrastructure.Context;

namespace Stefanini.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Commit()
    {
        var success = (await _context.SaveChangesAsync()) > 0;

        return success;
    }
}