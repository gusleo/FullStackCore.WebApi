using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Model
{
    public class RegisterModel
    {
        /// <summary>
        /// Register user username
        /// </summary>
        /// 
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; } = default!;

        /// <summary>
        /// Register user cellphone
        /// </summary>
        public string? CellPhone { get; set; }

        /// <summary>
        /// Register user email
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = default!;
        /// <summary>
        /// Register user password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = default!;

        /// <summary>
        /// Register user confirm password
        /// </summary>
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = default!;


    }
}
