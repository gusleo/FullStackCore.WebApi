using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Data.Entities.Abstract
{
    public interface IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        Guid Id { get; set; }

        Guid CreatedById { get; set; }
        Guid UpdatedById { get; set; }
        Guid? DeletedById { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        DateTime? DeletedDate { get; set; }
        bool IsDeleted { get; set; }    
    }
}
