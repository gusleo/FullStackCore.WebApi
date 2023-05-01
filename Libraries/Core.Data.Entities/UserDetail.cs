using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Scaffolding.Data.Entities.Abstract;
using Scaffolding.Auth.Entities;

namespace Core.Data.Entities
{
    public class UserDetail : WriteHistoryBase, IEntityBase
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = default!;


        [MaxLength(100)]
        public string? Avatar { get; set; } = default!;

        public int Point { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = default!;


    }
}