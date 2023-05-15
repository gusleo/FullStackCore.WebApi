using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Data.Repo.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Data.Repo
{
    public class EntityReadWriteBaseRepository<T> : EntityReadBaseRepository<T>, IDisposable, IReadBaseRepository<T>, IWriteBaseRepository<T> where T : class, IEntityBase, new()
    {

        public EntityReadWriteBaseRepository(IDataContext context) : base(context){ }
        public virtual void Add(T entity)
        {
            /*checking null dbEntityEntry, because on unit testing return null*/
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            if (dbEntityEntry != null && dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                _context.Set<T>().Add(entity);
            }

        }
        public virtual void AddRange(IEnumerable<T> entities)
        {

            _context.Set<T>().AddRange(entities);
        }

        public virtual void Edit(T entity)
        {
            /*EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            if ( dbEntityEntry.State == EntityState.Detached )
            {
                _context.Set<T>().Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;*/
            _context.Updated<T>(entity);

        }
        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            if (dbEntityEntry != null && dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }

        }
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

    }
}
