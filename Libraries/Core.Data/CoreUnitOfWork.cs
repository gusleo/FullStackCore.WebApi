using Core.Data.Infrastructure;
using Core.Data.Infrastructure.Repositories;
using Core.Data.Repositories;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Data.Repo;

namespace Core.Data;

public class CoreUnitOfWork : ICoreUnitOfWork
{
    #region Variable
    private readonly ICoreContext _context;
    private readonly IUserDetailRepository? userDetailRepository;
    #endregion

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="Exception"></exception>
    public CoreUnitOfWork(ICoreContext context)
    {
        _context = context ?? throw new Exception($"{nameof(ICoreContext)} is null");
    }

    public GenericRepository<T> Set<T>() where T : class, IEntityBase, new()
    {
        return new GenericRepository<T>(_context);
    }

    public int Commit()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IUserDetailRepository UserDetailRepository => userDetailRepository ?? new UserDetailRepository(_context);

}

