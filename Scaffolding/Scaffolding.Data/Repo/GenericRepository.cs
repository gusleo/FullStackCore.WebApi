using Scaffolding.Data.Entities.Abstract;

namespace Scaffolding.Data.Repo;

/// <summary>
/// Access general read write method
/// </summary>
/// <typeparam name="T"><see cref="IEntityBase"/></typeparam>
public class GenericRepository<T> : EntityReadWriteBaseRepository<T> where T : class, IEntityBase, new()
{
    public GenericRepository(IDataContext context) : base(context)
    {
    }
}