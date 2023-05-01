using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Model
{
    /// <summary>
    /// Model class of verify user by phone text
    /// </summary>
    public class VerifyPhoneCodeModel
    {
        public string PhoneNumber { get; set; } = default!;
        public string PhoneCode { get; set; } = default!;
    }
}
