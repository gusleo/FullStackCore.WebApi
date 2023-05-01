using Scaffolding.Service.Helper;
using Scaffolding.Service.Model;
using Scaffolding.Service.Model.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Core.Service.Models;

public class UserDetailModel : WriteHistoryBaseModel, IModelBase
{
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required, StringLength(100)]
    [RegularExpression(RegexCollection.NOT_ALLOW_SPECIAL_CHAR, ErrorMessage = RegexErrorMessage.SPECIAL_CHAR_NOT_ALLOWED)]
    public string FirstName { get; set; } = default!;

    [Required, StringLength(100)]
    [RegularExpression(RegexCollection.NOT_ALLOW_SPECIAL_CHAR, ErrorMessage = RegexErrorMessage.SPECIAL_CHAR_NOT_ALLOWED)]
    public string LastName { get; set; } = default!;

    [MaxLength(100)]
    public string Avatar { get; set; } = default!;

    public int Point { get; set; }


    public virtual UserModel User { get; private set; }
}
