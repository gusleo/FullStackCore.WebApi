using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scaffolding.Auth;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackCore.WebApi.Controllers;

public class BaseController : ControllerBase
{
    #region Variable
    protected readonly IAuthenticationService _authenticationService;
    #endregion

    protected BaseController(IAuthenticationService authenticationService)
    {
        this._authenticationService = authenticationService;
    }

    protected Guid GetUserId()
    {
        return _authenticationService.GetUserId();
    }
}


