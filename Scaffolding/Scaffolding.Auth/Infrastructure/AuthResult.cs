using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Infrastructure
{
    public class AuthResult
    {
        public const string ERROR = "Error";
        public const string INVALID_CODE = "Invalid code";
        public const string LOCKED_ACCOUNT = "User account locked";
        public const string SUCCESS = "Success";
        public const string USER_NOTFOUND = "User not found";
        public const string FAILED_GENERATE_TOKEN = "Failed generate token";

        public AuthResult()
        {
            Suceess = false;
            Message = ERROR;
        }
        public bool Suceess { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}
