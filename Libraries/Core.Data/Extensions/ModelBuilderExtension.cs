using Core.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scaffolding.Auth.Entities;
using Scaffolding.Auth.Infrastructure;

namespace Core.Data.Extensions;

/// <summary>
/// Model builder extension
/// </summary>
internal static class ModelBuilderExtension
{
    /// <summary>
    /// Generate seed data for database
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void Seed(this ModelBuilder modelBuilder)
    {
        #region Roles
        var superAdminRoleId = new Guid("f8bb088f-521b-4d0f-b26c-c354fa35ae98");
        var adminRoleId = new Guid("6f200df1-9ba6-406a-845b-10394f685726");
        var userRoleId = new Guid("0ad7cca0-4b59-4af2-9867-d04e85507170");

        modelBuilder.Entity<ApplicationRole>()
            .HasData(
                new ApplicationRole() { Id = superAdminRoleId, Name = MembershipConstant.SuperAdmin, NormalizedName = MembershipConstant.SuperAdmin.ToUpper() },
                new ApplicationRole() { Id = adminRoleId, Name = MembershipConstant.Admin, NormalizedName = MembershipConstant.Admin.ToUpper() },
                new ApplicationRole() { Id = userRoleId, Name = MembershipConstant.User, NormalizedName = MembershipConstant.User.ToUpper() }
            );
        #endregion

        #region User
        var password = new PasswordHasher<ApplicationUser>();

        var superAdminUserId = new Guid("3827f721-437f-4df2-a3ae-3eaa0e74b260");
        var superAdminUser = new ApplicationUser
        {
            Id = superAdminUserId,
            UserName = "admin@fullstackcore.com",
            NormalizedUserName = "admin@fullstackcore.com".ToUpper(),
            Email = "admin@fullstackcore.com",
            NormalizedEmail = "admin@fullstackcore.com".ToUpper(),
            EmailConfirmed = true,
            LockoutEnabled = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        superAdminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(superAdminUser, "test123!");

        var superAdminUserDetail = new UserDetail
        {
            Id = Guid.NewGuid(),
            FirstName = "Admin",
            LastName = "FullStack",
            UserId = superAdminUserId,
            CreatedById = superAdminUserId,
            UpdatedById = superAdminUserId,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        var intercomUserId = new Guid("60b49bae-77b6-4436-8fcb-6c41372db3e4");
        var intercomUser = new ApplicationUser
        {
            Id = intercomUserId,
            UserName = "intercom@fullstackcode.com",
            NormalizedUserName = "intercom@fullstackcode.com".ToUpper(),
            Email = "intercom@fullstackcode.com",
            NormalizedEmail = "intercom@fullstackcode.com".ToUpper(),
            EmailConfirmed = true,
            LockoutEnabled = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        intercomUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(intercomUser, "test123!"); ;

        var intercomUserDetail = new UserDetail
        {
            Id = Guid.NewGuid(),
            FirstName = "Intercom",
            LastName = "Fullstack",
            UserId = intercomUserId,
            CreatedById = superAdminUserId,
            UpdatedById = superAdminUserId,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };


        modelBuilder.Entity<ApplicationUser>()
            .HasData(superAdminUser, intercomUser);

        modelBuilder.Entity<AppUserRole>()
            .HasData(
                new AppUserRole() { RoleId = superAdminRoleId, UserId = superAdminUserId },
                new AppUserRole() { RoleId = adminRoleId, UserId = intercomUserId }
            );

        modelBuilder.Entity<UserDetail>()
            .HasData(superAdminUserDetail, intercomUserDetail);
        #endregion

    }
}
