using AutoMapper;
using Scaffolding.Data.Entities;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Infrastructure
{
    public class EntityModelMapper<TModel, TEntity>
        where TModel : IModelBase, new()
        where TEntity : IEntityBase, new()
    {
        protected readonly IMapper _mapper;
        public EntityModelMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Convert entity to model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected TModel GetModelFromEntity(TEntity entity)
        {
            return _mapper.Map<TModel>(entity);
        }

        /// <summary>
        /// Convert list entities to list model
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected IList<TModel> GetModelFromEntity(IList<TEntity> entities)
        {
            return _mapper.Map<IList<TModel>>(entities);
        }

        /// <summary>
        /// Convert list entity to list model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected IList<TM> GetModelFromEntity<T, TM>(IList<T> entities)
            where TM : IModelBase, new()
            where T : IEntityBase, new()
        {
            return _mapper.Map<IList<TM>>(entities);
        }

        /// <summary>
        /// Convert paging entity to paging model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected PaginationSet<TM> GetModelFromEntity<T, TM>(PaginationEntity<T> entities)
            where TM : IModelBase, new()
            where T : IEntityBase, new()
        {
            return _mapper.Map<PaginationSet<TM>>(entities);
        }

        /// <summary>
        /// Convert entity to model
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <typeparam name="TM">Type of model</typeparam>
        /// <param name="entity">Source of entity</param>
        /// <returns></returns>
        protected TM GetModelFromEntity<T, TM>(T entity)
            where TM : IModelBase, new()
            where T : IEntityBase, new()
        {
            return _mapper.Map<TM>(entity);
        }

        /// <summary>
        /// Convert paging entity to model
        /// </summary>
        /// <param name="entities">Pagination <see cref="PaginationEntity{T}"/></param>
        /// <returns></returns>
        protected PaginationSet<TModel> GetModelFromEntity(PaginationEntity<TEntity> entities)
        {
            return _mapper.Map<PaginationSet<TModel>>(entities);
        }

        /// <summary>
        /// Convert model to entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected TEntity GetEntityFromModel(TModel model)
        {
            return _mapper.Map<TEntity>(model);
        }
        protected T GetEntityFromModel<TM, T>(TM model)
            where TM : IModelBase, new()
            where T : IEntityBase, new()
        {
            return _mapper.Map<T>(model);
        }

        /// <summary>
        /// Convert list model to entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected IList<TEntity> GetEntityFromModel(IList<TModel> model)
        {
            return _mapper.Map<IList<TEntity>>(model);
        }

        /// <summary>
        /// Get list of model from list entities
        /// </summary>
        /// <typeparam name="T">destination</typeparam>
        /// <typeparam name="TM">source</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected IList<T> GetEntityFromModel<TM, T>(IList<TM> model)
            where TM : IModelBase, new()
            where T : IEntityBase, new()
        {
            return _mapper.Map<IList<T>>(model);
        }

        /// <summary>
        /// Convert list model to entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected PaginationEntity<TEntity> GetEntityFromModel(PaginationSet<TModel> model)
        {
            return _mapper.Map<PaginationEntity<TEntity>>(model);
        }

        /// <summary>
        /// Convert model to entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected TEntity GetEntityFromModel(TModel model, TEntity entity)
        {
            return _mapper.Map<TModel, TEntity>(model, entity);
        }


    }
}
