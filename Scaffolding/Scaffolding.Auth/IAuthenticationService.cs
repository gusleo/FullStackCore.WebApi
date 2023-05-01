using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Scaffolding.Auth.Entities;
using Scaffolding.Auth.Infrastructure;
using Scaffolding.Auth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth
{
    /// <summary>
    /// Authentication services for user
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Get current user is authenticated
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticate();

        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser?> GetCurrentUserAsync();

        /// <summary>
        /// Get user IsSuperAdmin
        /// </summary>
        /// <returns></returns>
        bool IsSuperAdmin();

        /// <summary>
        /// Get user role
        /// </summary>
        /// <returns></returns>
        string GetUserRole();


        /// <summary>
        /// Register new user by email
        /// </summary>
        /// <param name="model">Register user data <see cref="RegisterModel"/></param>
        /// <param name="emailConfirmed">Is verify by email</param>
        //// <param name="smsConfirmed">Is verify by sms</param>
        /// <returns></returns>
        Task<AuthenticationResult> RegisterAsync(RegisterModel model, bool emailConfirmed, bool smsConfirmed);

        /// <summary>
        /// Register user with phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterWithPhoneNumberAsync(string phoneNumber);


        /// <summary>
        /// Get UserID of current logged user
        /// </summary>
        /// <returns></returns>
        Guid GetUserId();

        /// <summary>
        /// User confirm email
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="code">Token</param>
        /// <returns></returns>
        Task<AuthResult> ConfirmEmailAsync(string userId, string code);

        /// <summary>
        /// Check user email is confirmed
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        Task<bool> IsEmailConfirmedAsync(string userId);

        /// <summary>
        /// Method to log in user
        /// </summary>
        /// <param name="model">Model of login <see cref="LoginModel"/></param>
        /// <param name="lockoutOnFailure">Is user lockout</param>
        /// <returns></returns>
        Task<SignInResult> LoginAsync(LoginModel model, bool lockoutOnFailure = false);

        /// <summary>
        /// Method to log out user
        /// </summary>
        /// <returns></returns>
        Task<AuthResult> LogoutAsync();

        /// <summary>
        /// Method configure external login
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        AuthenticationProperties ConfigureExternalAuthentication(string provider, string returnUrl);

        /// <summary>
        /// Provide information from external login 
        /// </summary>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        Task<ExternalLoginInfoModel?> GetExternalLoginAsync(bool isPersistent = false);

        /// <summary>
        /// Provide token to verify
        /// </summary>
        /// <param name="phoneNumber">User phone number</param>
        /// <returns></returns>
        Task<AuthResult> RequestPhoneTokenAsync(string phoneNumber);

        /// <summary>
        /// Checking user phone number verify code
        /// </summary>
        /// <param name="model">Model of <see cref="VerifyPhoneCodeModel"/></param>
        /// <returns></returns>
        Task<AuthResult> VerifyPhoneCodeAsync(VerifyPhoneCodeModel model);

        /// <summary>
        /// Checking user email verify code
        /// </summary>
        /// <param name="model">Modle of <see cref="VerifyCodeModel"/></param>
        /// <returns></returns>
        Task<AuthResult> VerifyCodeAsync(VerifyCodeModel model);

        /// <summary>
        /// Add user emial
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <returns></returns>
        Task<AuthResult> AddEmailAsync(string email);

        /// <summary>
        /// Provide activation code
        /// </summary>
        /// <param name="provider">Provider to send</param>
        /// <param name="receiver">Receiver</param>
        /// <returns></returns>
        Task<AuthResult> GenerateActivationCodeAsync(string provider, string receiver);

        /// <summary>
        /// Assign role to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<AuthResult> AssignRoleToUserAsync(Guid userId, string[] roles);

        /// <summary>
        /// Provide all role available
        /// </summary>
        /// <returns></returns>
        Task<IList<ApplicationRole>> GetAvailableRoleAsync();

        /// <summary>
        /// Provide current user role login
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<string>> GetUserRoleAsync(int userId);

        /// <summary>
        /// Provide user claims
        /// </summary>
        /// <returns></returns>
        Task<IList<Claim>> GetClaimsAsync();

        /// <summary>
        /// Delete user and assigned roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AuthResult> DeleteAsync(Guid userId);

        /// <summary>
        /// Lock user by date
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<bool> LockUserAsync(Guid userId, DateTime? endDate);

        /// <summary>
        /// Unlock user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UnlockUserAsync(Guid userId);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<AuthResult> GetUserByUsername(string username);

    }
}
