using Core.Data.Entities;
using Core.Service.Models;
using Scaffolding.Service.Infrastructure;
using Scaffolding.Service.Infrastructure.Abstract;

namespace Core.Service.Infrastructure;

public interface IUserDetailService : IReadWriteService<UserDetailModel, UserDetail>
{
    Task<Response<PaginationSet<UserDetailModel>>> FindUserAsync(string clue, int page, int pageSize);
    Task<Response<UserDetailModel>> GetDetailAsync(Guid userId);
}
