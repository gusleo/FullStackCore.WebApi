using Microsoft.EntityFrameworkCore;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Data.Entities;
using Scaffolding.Data.Repo.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Data.Repo
{
    public class EntityReadBaseRepository<T> : IDisposable, IReadBaseRepository<T> where T : class, IEntityBase, new()
    {
        private bool disposed = false;
        protected readonly IDataContext _context;


        public EntityReadBaseRepository(IDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(IDataContext));
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public virtual async Task<PaginationEntity<T>> GetAllAsync(int pageIndex, int pageSize)
        {
            IQueryable<T> query = _context.Set<T>();
            query = query.OrderByDescending(x => x.CreatedDate);
            return new PaginationEntity<T>()
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await query.CountAsync(),
                Items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
        public virtual async Task<PaginationEntity<T>> GetAllAsync(int pageIndex, int pageSize, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            query = query.OrderByDescending(x => x.CreatedDate);
            return new PaginationEntity<T>()
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await query.CountAsync(),
                Items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public virtual async Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            query = query.OrderByDescending(x => x.CreatedDate);
            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<T?> GetSingleAsync(Guid id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }
        public virtual T? GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }
        public virtual T? GetSingle(Guid id)
        {
            return _context.Set<T>().FirstOrDefault(e => e.Id == id);
        }
        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            query = query.OrderByDescending(x => x.CreatedDate);
            return await query.Where(predicate).ToListAsync();
        }
        
        public virtual async Task<PaginationEntity<T>> FindByAsync(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            //set predicate 
            query = query.Where(predicate);
            query = query.OrderByDescending(x => x.CreatedDate);
            return new PaginationEntity<T>()
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await query.CountAsync(),
                Items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
