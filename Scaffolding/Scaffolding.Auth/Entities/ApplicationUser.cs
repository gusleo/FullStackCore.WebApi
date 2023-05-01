using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scaffolding.Auth.Infrastructure;

namespace Scaffolding.Auth.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //public UserStatus Status { get; set; } = UserStatus.Active;
        //public string? ProviderSubjectId { get; set; } = default!;
        //public string? ProviderName { get; set; } = default!;

        //public string? RefreshToken { get; set; }
        //public DateTime RefreshTokenExpiryTime { get; set; }
    }


    public class AppUserLogin : IdentityUserLogin<Guid> { }


    public class AppUserRole : IdentityUserRole<Guid> { }

    public class AppUserClaim : IdentityUserClaim<Guid> { }

    public class AppUserToken : IdentityUserToken<Guid> { }


    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
    }

    public class CustomRoleStore : RoleStore<ApplicationRole, DbContext, Guid>
    {
        public CustomRoleStore(DbContext context) : base(context) { }
    }

    public class CustomUserStore : UserStore<ApplicationUser, ApplicationRole, DbContext, Guid>
    {
        public CustomUserStore(DbContext context)
            : base(context)
        {
        }
    }
}
