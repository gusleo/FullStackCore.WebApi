using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Model
{
    /// <summary>
    /// Model class of identity
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="result">Identity of <see cref="IdentityResult"/></param>
        public AuthenticationResult(IdentityResult result)
        {
            this.Result = result;
            GeneratedToken = String.Empty;
            GenerateSMSToken = String.Empty;    
        }
        public IdentityResult Result { get; set; }
        public Guid UserId { get; set; }
        public string GeneratedToken { get; set; }
        public string GenerateSMSToken { get; set; }
    }
}
