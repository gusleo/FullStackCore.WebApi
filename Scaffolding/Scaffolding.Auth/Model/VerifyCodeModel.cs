using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scaffolding.Auth.Model
{
    /// <summary>
    /// Model class of verify user by email
    /// </summary>
    public class VerifyCodeModel
    {
        [Required]
        public string Provider { get; set; } = default!;

        [Required]
        public string Code { get; set; } = default!;

        public string ReturnUrl { get; set; } = default!;

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
