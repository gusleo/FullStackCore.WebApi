using Scaffolding.Service.Infrastructure;
using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Model
{
    public class UserModel : WriteHistoryBaseModel, IModelBase
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public UserStatus Status { get; set; }
        public int AccessFailedCount { get; set; }
        public string ProviderName { get; set; } = default!;

        // not mapped to database
        [Required]
        public string FirstName { get; set; } = default!;
        [Required]
        public string LastName { get; set; } = default!;
    }
}
