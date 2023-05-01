using Core.Data.Entities;
using Core.Service.Models;
using Scaffolding.Service.Infrastructure.Abstract;

namespace Core.Service.Infrastructure;

public interface ILogEntryService : IReadWriteService<LogEntryModel, LogEntry>
{
}
