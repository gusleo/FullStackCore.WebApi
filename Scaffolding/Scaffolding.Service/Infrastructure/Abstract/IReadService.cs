using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Infrastructure.Abstract
{
    public interface IReadService<TModel> : IDisposable
        where TModel : IModelBase, new()
    {
        /// <summary>
        /// Get single model by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        Task<Response<TModel>> GetSingleAsync(Guid id);
        /// <summary>
        /// Get all data paginated
        /// </summary>
        /// <returns></returns>
        Task<Response<PaginationSet<TModel>>> GetAllAsync(int pageIndex, int pageSize = Constant.PAGE_SIZE);

        /// <summary>
        /// Get all data
        /// </summary>
        /// <returns></returns>
        Task<Response<IList<TModel>>> GetAllAsync();

    }
}
