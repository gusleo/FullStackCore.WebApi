using Scaffolding.Service.Helper;
using Scaffolding.Service.Model.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Core.Service.Models;

public class LogEntryModel : WriteHistoryBaseModel, IModelBase
{
    public Guid Id { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = default!;

    [Required]
    [RegularExpression(RegexCollection.NOT_ALLOW_SPECIAL_CHAR, ErrorMessage = RegexErrorMessage.SPECIAL_CHAR_NOT_ALLOWED)]
    public string Message { get; set; } = default!;

    [Required]
    [RegularExpression(RegexCollection.NOT_ALLOW_SPECIAL_CHAR, ErrorMessage = RegexErrorMessage.SPECIAL_CHAR_NOT_ALLOWED)]
    public string? Exception { get; set; } = default!;
}
