using Core.Service.Infrastructure;
using Microsoft.Extensions.Logging;

namespace FullStackCore.WebApi.Logger;

public class DatabaseLoggerProvider : ILoggerProvider
{
    private readonly ILogEntryService _logEntryService;
    public DatabaseLoggerProvider(ILogEntryService logEntryService)
    {
        _logEntryService = logEntryService;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DatabaseLogger(_logEntryService);
    }

    public void Dispose()
    {
        // Cleanup resources here if needed
    }
}
