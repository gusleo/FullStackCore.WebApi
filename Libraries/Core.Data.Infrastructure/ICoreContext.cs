using Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Scaffolding.Data;

namespace Core.Data.Infrastructure;

public interface ICoreContext : IDataContext
{
    DbSet<UserDetail> UserDetails { get; set; }
    DbSet<LogEntry> LogEntries { get; set; }
}
