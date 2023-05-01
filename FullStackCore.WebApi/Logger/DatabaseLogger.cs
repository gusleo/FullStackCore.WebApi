using Core.Service.Infrastructure;
using Core.Service.Models;

namespace FullStackCore.WebApi.Logger;

public class DatabaseLogger : ILogger
{
    private readonly ILogEntryService _logEntryService;

    public DatabaseLogger(ILogEntryService logEntryService)
    {
        _logEntryService = logEntryService;
    }

    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        _logEntryService.Create(new LogEntryModel
        {
            Timestamp = DateTime.UtcNow,
            Level = logLevel.ToString(),
            Message = message,
            Exception = exception?.ToString()
        });        
    }
}




