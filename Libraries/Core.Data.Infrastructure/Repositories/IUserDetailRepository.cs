using Core.Data.Entities;
using Scaffolding.Data.Repo.Abstract;

namespace Core.Data.Infrastructure.Repositories;

public interface IUserDetailRepository : IWriteBaseRepository<UserDetail>, IReadBaseRepository<UserDetail>
{
}
