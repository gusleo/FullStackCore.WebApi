using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Scaffolding.Data.Entities.Abstract;

namespace Scaffolding.Data
{
    public interface IDataContext
    {

        DatabaseFacade Database { get; }

        /// <summary>
        /// Save all entity changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();


        /// <summary>
        /// Set entity into database table
        /// </summary>
        /// <typeparam name="TEntity">Entity class derived from <see cref="IEntityBase"/></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Provide access to change tracking
        /// </summary>
        /// <typeparam name="TEntity">Entity class derived from <see cref="IEntityBase"/></typeparam>
        /// <returns></returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Update information to database
        /// </summary>
        /// <typeparam name="TEntity">Entity class derived from <see cref="IEntityBase"/></typeparam>
        /// <param name="entity">Entity class</param>
        void Updated<TEntity>(TEntity entity) where TEntity : class, IEntityBase;

        /// <summary>
        /// Disposable connection
        /// </summary>
        void Dispose();
    }
}
