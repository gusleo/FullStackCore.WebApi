using AutoMapper;
using Scaffolding.Auth;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Infrastructure
{
    /// <summary>
    /// Base class of IReadWriteService
    /// </summary>
    /// <remarks>
    /// We don't want to DRY consistent method 
    /// for each class implement IReadWriteService
    /// </remarks>
    /// <typeparam name="TModel"><see cref="IModelBase"/></typeparam>
    /// <typeparam name="TEntity"><see cref="IEntityBase"/></typeparam>
    public class ReadWriteServiceBase<TModel, TEntity> : EntityModelMapper<TModel, TEntity>
        where TModel : IModelBase, new()
        where TEntity : IEntityBase, new()
    {
        
        protected readonly IAuthenticationService _authService;
        public ReadWriteServiceBase(IAuthenticationService authService, IMapper mapper) : base(mapper)
        {
            _authService = authService;
        }

        /// <summary>
        /// Check userId
        /// </summary>
        /// <returns></returns>
        public Guid GetUserId()
        {
            return _authService.GetUserId();
        }

        /// <summary>
        /// Check if user is SuperAdmin
        /// </summary>
        /// <returns></returns>
        public bool IsSuperAdmin()
        {
            return _authService.IsSuperAdmin();
        }


        /// <summary>
        /// Generate error response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        protected Response<T> InitErrorResponse<T>() where T : IModelBase
        {
            return new Response<T>()
            {
                Success = false,
                StatusCode = 500,
                Message = MessageConstant.Error
            };
        }
        /// <summary>
        /// Generate error response
        /// </summary>
        /// <returns></returns>
        protected Response<TModel> InitErrorResponse()
        {
            return new Response<TModel>()
            {
                Success = false,
                StatusCode = 500,
                Message = MessageConstant.Error
            };
        }
        /// <summary>
        /// Generate error response
        /// </summary>
        /// <returns></returns>
        protected Response<IList<TModel>> InitErrorListResponse()
        {
            return new Response<IList<TModel>>()
            {
                Success = false,
                StatusCode = 500,
                Message = MessageConstant.Error
            };
        }
        /// <summary>
        /// Generate error response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Response<IList<T>> InitErrorListResponse<T>()
        {
            return new Response<IList<T>>()
            {
                Success = false,
                StatusCode = 500,
                Message = MessageConstant.Error
            };
        }
        /// <summary>
        /// Generate error response
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected Response<PaginationSet<TModel>> InitErrorResponse(int pageIndex, int pageSize)
        {
            return new Response<PaginationSet<TModel>>()
            {
                Success = false,
                StatusCode = 500,
                Message = MessageConstant.Error,
                Item = new PaginationSet<TModel>()
                {
                    Page = pageIndex,
                    PageSize = pageSize
                }
            };
        }
        /// <summary>
        /// Generate error response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected Response<PaginationSet<T>> InitErrorResponse<T>(int pageIndex, int pageSize)
            where T : IModelBase, new()
        {
            return new Response<PaginationSet<T>>()
            {
                Success = false,
                StatusCode = 500,
                Message = MessageConstant.Error,
                Item = new PaginationSet<T>()
                {
                    Page = pageIndex,
                    PageSize = pageSize
                }
            };
        }
        /// <summary>
        /// Generate success response
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Response<TModel> InitSuccessResponse(string message)
        {
            return new Response<TModel>()
            {
                Success = true,
                StatusCode = 200,
                Message = message
            };
        }
        /// <summary>
        /// Generate success response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Response<T> InitSuccessResponse<T>(string message)
        {
            return new Response<T>()
            {
                Success = true,
                StatusCode = 200,
                Message = message
            };
        }
        /// <summary>
        /// Generate success response
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Response<IList<TModel>> InitSuccessListResponse(string message)
        {
            return new Response<IList<TModel>>()
            {
                Success = true,
                StatusCode = 200,
                Message = message
            };
        }
        /// <summary>
        /// Generate success response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Response<IList<T>> InitSuccessListResponse<T>(string message)
        {
            return new Response<IList<T>>()
            {
                Success = true,
                StatusCode = 200,
                Message = message
            };
        }
        /// <summary>
        /// Generate success response
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Response<PaginationSet<TModel>> InitSuccessResponse(int pageIndex, int pageSize, string message)
        {
            return new Response<PaginationSet<TModel>>
            {
                Success = true,
                Message = message,
                StatusCode = 200,
                Item = new PaginationSet<TModel>()
                {
                    Page = pageIndex,
                    PageSize = pageSize
                }
            };
        }
        /// <summary>
        /// Generate success response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected Response<PaginationSet<T>> InitSuccessResponse<T>(int pageIndex, int pageSize, string message)
            where T : IModelBase, new()
        {
            return new Response<PaginationSet<T>>
            {
                Success = true,
                Message = message,
                StatusCode = 200,
                Item = new PaginationSet<T>()
                {
                    Page = pageIndex,
                    PageSize = pageSize
                }
            };
        }

    }
}
