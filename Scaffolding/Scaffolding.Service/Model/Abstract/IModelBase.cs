using System.ComponentModel.DataAnnotations;

namespace Scaffolding.Service.Model.Abstract
{
    public interface IModelBase
    {
        [Required]
        Guid Id { get; set; }

        Guid CreatedById { get; }
        Guid UpdatedById { get; }
        DateTime CreatedDate { get; }
        DateTime UpdatedDate { get; }
        DateTime? DeletedDate { get; }
        bool IsDeleted { get; }
        Guid? DeletedById { get; }
    }
}
