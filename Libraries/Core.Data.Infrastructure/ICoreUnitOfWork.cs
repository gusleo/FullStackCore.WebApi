using Core.Data.Infrastructure.Repositories;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Data.Repo;

namespace Core.Data.Infrastructure;

public interface ICoreUnitOfWork : IDisposable
{
    // <summary>
    /// Access general read write method of repository
    /// </summary>
    /// <typeparam name="T"><see cref="IEntityBase"/></typeparam>
    /// <returns></returns>
    GenericRepository<T> Set<T>() where T : class, IEntityBase, new();
    
    /// <summary>
    /// Access to user detail repository
    /// </summary>
    IUserDetailRepository UserDetailRepository { get; }
    int Commit();
}
