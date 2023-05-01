using AutoMapper;
using Core.Data.Entities;
using Core.Data.Infrastructure;
using Core.Service.Infrastructure;
using Core.Service.Models;
using Core.Service.Services.Base;
using Scaffolding.Auth;

namespace Core.Service.Services;

public class LogEntryService : GenericService<LogEntryModel, LogEntry>, ILogEntryService
{
    public LogEntryService(ICoreUnitOfWork coreUnitOfWork, IAuthenticationService authService, IMapper mapper) : base(coreUnitOfWork, authService, mapper)
    {
    }
}
