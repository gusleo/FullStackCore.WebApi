using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Service.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scaffolding.Auth;
using Scaffolding.Auth.Model;
using Scaffolding.Service.Infrastructure;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackCore.WebApi.Controllers.Common.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Authorize]
public class UserController : BaseController
{
    #region Variable
    private readonly IUserDetailService _userDetailService;
    #endregion

    #region Constructor
    public UserController(IAuthenticationService authenticationService, IUserDetailService userDetailService) : base(authenticationService)
    {
        this._userDetailService = userDetailService;
    }
    #endregion

    #region Public

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="model"><see cref="RegisterModel"/></param>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _authenticationService.GetUserByUsername(model.Username);
        if (userExists.Suceess)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<string> { Success = false, StatusCode = 500, Message = "User already exist!" });
        
        var result = await _authenticationService.RegisterAsync(model, true, false);
        if (!result.Result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<string> { Success = false, StatusCode = 500, Message = "User creation failed! Please check user details and try again." });

        // TODO : send email confirmation
        return Ok(new Response<string> { Success = true, StatusCode = 200, Message = "User created successfully!" });
    }

    /// <summary>
    /// Get detail info for logged user
    /// </summary>
    /// <returns></returns>
    [HttpGet("info")]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = _authenticationService.GetUserId();
        var result = await _userDetailService.GetDetailAsync(userId);
        return Ok(result);
    }
    #endregion
}

