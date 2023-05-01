using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Model
{
    /// <summary>
    /// Login class information
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string? Password { get; set; } = default!;
        public bool RememberLogin { get; set; }
    }
}
