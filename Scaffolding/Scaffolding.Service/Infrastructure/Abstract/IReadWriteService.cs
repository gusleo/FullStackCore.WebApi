using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Infrastructure.Abstract
{
    public interface IReadWriteService<TModel, TEntity> : IReadService<TModel>
        where TModel : IModelBase, new()
        where TEntity : IEntityBase, new()
    {
        
        /// <summary>
        /// Create model to database
        /// </summary>
        /// <param name="modelToCreate">T variable</param>
        /// <returns></returns>
        Response<TModel> Create(TModel modelToCreate);

        /// <summary>
        /// Edit model to database
        /// </summary>
        /// <param name="modelToEdit">T variable</param>
        /// <returns></returns>
        Task<Response<TModel>> EditAsync(TModel modelToEdit);
        /// <summary>
        /// Delete model to database
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        Task<Response<TModel>> DeleteAsync(Guid id);

        
        /// <summary>
        /// Prevent child insertion
        /// with remove the child manualy
        /// </summary>
        /// <param name="entity"></param>
        TEntity RemoveChildEntity(TEntity entity);

    }
}
