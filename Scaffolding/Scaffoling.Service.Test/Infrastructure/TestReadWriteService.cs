using AutoMapper;
using Scaffolding.Auth;
using Scaffolding.Data.Test.Entities;
using Scaffolding.Service.Infrastructure;
using Scaffolding.Service.Model.Abstract;
using Scaffoling.Service.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffoling.Service.Test.Infrastructure
{
    public class TestReadWriteService : ReadWriteServiceBase<TestModel, TestEntity>
    {
        public TestReadWriteService(IAuthenticationService authService, IMapper mapper) : base(authService, mapper)
        {
        }

        public new Response<T> TestInitErrorResponse<T>() where T : IModelBase
        {
            return InitErrorResponse<T>();
        }

        public new Response<IList<T>> TestInitErrorListResponse<T>()
        {
            return InitErrorListResponse<T>();
        }

        public new Response<PaginationSet<T>> TestInitErrorResponse<T>(int pageIndex, int pageSize) where T : IModelBase, new()
        {
            return InitErrorResponse<T>(pageIndex, pageSize);
        }
        public new Response<TModel> TestInitSuccessResponse<TModel>(string message)
        {
            return InitSuccessResponse<TModel>(message);
        }

        
        public new Response<IList<TModel>> TestInitSuccessListResponse<TModel>(string message)
        {
            return InitSuccessListResponse<TModel>(message);
        }

        
        public new Response<PaginationSet<T>> TestInitSuccessResponse<T>(int pageIndex, int pageSize, string message)
            where T : IModelBase, new()
        {
            return InitSuccessResponse<T>(pageIndex, pageSize, message);
        }


        // Implement other protected methods for testing if necessary
    }
}
