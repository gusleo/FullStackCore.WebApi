using AutoMapper;
using Core.Data.Infrastructure;
using Scaffolding.Auth;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Service.Infrastructure;
using Scaffolding.Service.Infrastructure.Abstract;
using Scaffolding.Service.Model.Abstract;

namespace Core.Service.Services.Base;

public class GenericService<TModel, TEntity> : ReadWriteServiceBase<TModel, TEntity>, IReadWriteService<TModel, TEntity>
        where TModel : class, IModelBase, new()
        where TEntity : class, IEntityBase, new()
{
    protected readonly ICoreUnitOfWork _coreUnitOfWork;
    public GenericService(ICoreUnitOfWork coreUnitOfWork, IAuthenticationService authService, IMapper mapper) : base(authService, mapper)
    {
        _coreUnitOfWork = coreUnitOfWork;
    }

    public virtual Response<TModel> Create(TModel modelToCreate)
    {
        var response = InitErrorResponse<TModel>();
        var en = GetEntityFromModel(modelToCreate);
        if (en != null)
        {

            _coreUnitOfWork.Set<TEntity>().Add(en);
            _coreUnitOfWork.Commit();

            response.Success = true;
            response.Message = MessageConstant.Create;
            response.Item = GetModelFromEntity(en);
        }
        else
        {
            response.Message = MessageConstant.Error;
        }

        return response;

    }

    public virtual async Task<Response<TModel>> DeleteAsync(Guid id)
    {
        var response = InitErrorResponse<TModel>();
        var en = await _coreUnitOfWork.Set<TEntity>().GetSingleAsync(x => x.Id == id);
        if (en != null)
        {
            _coreUnitOfWork.Set<TEntity>().Delete(en);
            _coreUnitOfWork.Commit();
            response.Success = true;
            response.Message = MessageConstant.Delete;
            response.Item = GetModelFromEntity(en);
        }
        else
        {
            response.Message = MessageConstant.NotFound;
        }

        return response;
    }

    public void Dispose()
    {
        _coreUnitOfWork.Dispose();
    }

    public virtual async Task<Response<TModel>> EditAsync(TModel modelToEdit)
    {
        var response = InitErrorResponse<TModel>();
        var en = await _coreUnitOfWork.Set<TEntity>().GetSingleAsync(x => x.Id == modelToEdit.Id);
        if (en is not null)
        {
            var entity = GetEntityFromModel(modelToEdit);

            _coreUnitOfWork.Set<TEntity>().Edit(entity);
            _coreUnitOfWork.Commit();

            response.Success = true;
            response.Message = MessageConstant.Delete;
            response.Item = modelToEdit;
        }
        else
        {
            response.Message = MessageConstant.NotFound;
        }
        return response;
    }

    public virtual async Task<Response<PaginationSet<TModel>>> GetAllAsync(int pageIndex, int pageSize = 20)
    {
        var response = InitSuccessResponse<TModel>(pageIndex, pageSize, MessageConstant.Load);
        var items = await _coreUnitOfWork.Set<TEntity>().GetAllAsync(pageIndex, pageSize);
        response.Item = GetModelFromEntity(items);
        return response;
    }

    public virtual async Task<Response<IList<TModel>>> GetAllAsync()
    {
        var response = InitSuccessListResponse<TModel>(MessageConstant.Load);
        var items = await _coreUnitOfWork.Set<TEntity>().GetAllAsync();
        response.Item = GetModelFromEntity(items.ToList());
        return response;
    }

    public virtual async Task<Response<TModel>> GetSingleAsync(Guid id)
    {
        var response = InitErrorResponse<TModel>();
        var en = await _coreUnitOfWork.Set<TEntity>().GetSingleAsync(x => x.Id == id);
        if (en != null)
        {
            response.Success = true;
            response.Message = MessageConstant.Load;
            response.Item = GetModelFromEntity(en);
        }
        else
        {
            response.Message = MessageConstant.NotFound;
        }

        return response;
    }

    public virtual TEntity RemoveChildEntity(TEntity entity)
    {
        return entity;
    }
}