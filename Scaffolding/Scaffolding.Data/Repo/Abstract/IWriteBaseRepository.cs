using Scaffolding.Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Data.Repo.Abstract
{

    /// <summary>
    /// Class to write data to database
    /// </summary>
    /// <typeparam name="T">Entity class derived from <see cref="IEntityBase"/></typeparam>
    public interface IWriteBaseRepository<T> where T : class, IEntityBase, new()
    {
        /// <summary>
        /// Create data to database
        /// </summary>
        /// <param name="entity">Entity class</param>
        void Add(T entity);

        /// <summary>
        /// Create multiple data to database
        /// </summary>
        /// <param name="entitites">List of entities</param>
        void AddRange(IEnumerable<T> entitites);

        /// <summary>
        /// Delete data from database
        /// </summary>
        /// <param name="entity">Entity class</param>
        void Delete(T entity);

        /// <summary>
        /// Delete multiple data from database
        /// </summary>
        /// <param name="entitites">List of entities</param>
        void DeleteRange(IEnumerable<T> entitites);

        /// <summary>
        /// Edit data from database
        /// </summary>
        /// <param name="entity">Entity class</param>
        void Edit(T entity);
    }
}
