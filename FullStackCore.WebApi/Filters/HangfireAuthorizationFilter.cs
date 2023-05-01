using Hangfire.Dashboard;

namespace FullStackCore.WebApi.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HangfireAuthorizationFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // Check if the user is authenticated with OpenIddict.
        if (!httpContext.User.Identity.IsAuthenticated)
        {
            // Redirect the user to the OpenIddict authorization endpoint.
            httpContext.Response.Redirect("/connect/authorize");
            return false;
        }

        // Check if the user has the necessary role or permission to access the dashboard.
        // Replace this with your own authorization logic.
        if (!httpContext.User.IsInRole("admin"))
        {
            httpContext.Response.StatusCode = 401; // Unauthorized
            return false;
        }

        // The user is authorized to access the dashboard.
        return true;
    }
}

