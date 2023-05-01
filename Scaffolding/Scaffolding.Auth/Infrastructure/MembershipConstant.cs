using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Infrastructure
{
    public static class MembershipConstant
    {
        public const string Member = "Member";
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string RolesAdmin = "SuperAdmin,Admin";

        public const string EmailBlastEditor = "EmailBlastEditor";
        public const string ArticleEditor = "ArticleEditor";
        public const string PromotionEditor = "PromotionEditor";
        public const string MyAppEditor = "MyAppEditor";
        public const string PollingEditor = "PollingEditor";

    }
}
