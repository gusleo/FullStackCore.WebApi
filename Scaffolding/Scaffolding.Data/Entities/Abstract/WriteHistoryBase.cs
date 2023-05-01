using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffolding.Auth.Entities;

namespace Scaffolding.Data.Entities.Abstract
{
    /// <summary>
    /// Tracking base class
    /// </summary>
    public abstract class WriteHistoryBase
    {
        [Required]
        public Guid CreatedById { get; set; }

        [Required]
        public Guid UpdatedById { get; set; }

        public Guid? DeletedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public virtual ApplicationUser CreatedByUser { get; set; } = default!;

        [ForeignKey(nameof(UpdatedById))]
        public virtual ApplicationUser UpdatedByUser { get; set; } = default!;

        [ForeignKey(nameof(DeletedById))]
        public virtual ApplicationUser? DeletedByUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
