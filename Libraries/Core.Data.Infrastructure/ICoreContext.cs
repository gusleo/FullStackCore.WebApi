using Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Scaffolding.Data;

namespace Core.Data.Infrastructure;

public interface ICoreContext : IDataContext
{
    DbSet<UserDetail> UserDetails { get; }
    DbSet<LogEntry> LogEntries { get; }
}
