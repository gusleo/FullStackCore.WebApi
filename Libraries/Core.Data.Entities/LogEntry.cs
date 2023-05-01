using Scaffolding.Data.Entities.Abstract;

namespace Core.Data.Entities;

public class LogEntry : WriteHistoryBase, IEntityBase
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Exception { get; set; } = default!;
}
