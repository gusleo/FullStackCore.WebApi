using Core.Data.Entities;
using Core.Data.Infrastructure;
using Core.Data.Infrastructure.Repositories;
using Scaffolding.Data.Repo;

namespace Core.Data.Repositories;

public class UserDetailRepository : EntityReadWriteBaseRepository<UserDetail>, IUserDetailRepository
{
    public UserDetailRepository(ICoreContext context) : base(context)
    {
    }
}