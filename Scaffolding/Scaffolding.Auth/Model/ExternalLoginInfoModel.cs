using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Model
{
    /// <summary>
    /// External login class information
    /// </summary>
    public class ExternalLoginInfoModel
    {
        /// <summary>
        /// External login information
        /// </summary>
        public ExternalLoginInfo? Info { get; set; }

        /// <summary>
        /// SignIn information
        /// </summary>
        public SignInResult? Result { get; set; }
    }
}
