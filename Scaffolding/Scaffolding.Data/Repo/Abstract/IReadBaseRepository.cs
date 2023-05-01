using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Scaffolding.Data.Entities;
using Scaffolding.Data.Entities.Abstract;

namespace Scaffolding.Data.Repo.Abstract
{
    /// <summary>
    /// Base class to read data from database layer
    /// </summary>
    /// <typeparam name="T">Entity class derifed from <see cref="IEntityBase" /></typeparam>
    public interface IReadBaseRepository<T> where T : class, IEntityBase, new()
    {

        /// <summary>
        /// Async method to get all data with full join with other entities
        /// </summary>
        /// <param name="includeProperties">Linq join expression</param>
        /// <returns></returns>
        Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Async method to get all of data async
        /// </summary>       
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Async method to get all of data with paging
        /// </summary>
        /// <param name="pageIndex">Index of page</param>
        /// <param name="pageSize">Size of page</param>
        /// <returns><see cref="PaginationEntity{T}"/> pagination</returns>
        Task<PaginationEntity<T>> GetAllAsync(int pageIndex, int pageSize);

        /// <summary>
        /// Async method to get all of data with paging
        /// </summary>
        /// <param name="pageIndex">Index of page</param>
        /// <param name="pageSize">Size of page</param>
        /// <returns><see cref="PaginationEntity{T}"/> pagination</returns>
        Task<PaginationEntity<T>> GetAllAsync(int pageIndex, int pageSize, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Async method to get all data by Linq expression
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Async method to get all data by predicate with full join
        /// </summary>
        /// <param name="predicate">Linq expiression</param>
        /// <param name="includeProperties">Linq join expression</param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Async method to get all paging data by predicate with Linq join 
        /// </summary>
        /// <param name="predicate">Linq expiression</param>
        /// <param name="pageIndex">Index of page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="includeProperties">Linq join expression</param>
        /// <returns></returns>
        Task<PaginationEntity<T>> FindByAsync(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Async method to get data by primary key
        /// </summary>
        /// <param name="id">Primary key of entity</param>
        /// <returns></returns>
        Task<T?> GetSingleAsync(Guid id);

        /// <summary>
        /// Async method to get single data by predicate
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <returns></returns>
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Async method to get data by predicate with Linq expression and join
        /// </summary>
        /// <param name="predicate">Linq expiression</param>
        /// <param name="includeProperties">Linq join expression</param>
        /// <returns></returns>
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Method to get single data by linq expiression
        /// </summary>
        /// <param name="predicate">Linq expiression</param>
        /// <returns></returns>
        T? GetSingle(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get single data by primary key
        /// </summary>
        /// <param name="id">Primary key of entity</param>
        /// <returns></returns>
        T? GetSingle(Guid id);
    }
}
