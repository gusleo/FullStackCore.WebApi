using Hangfire.Dashboard;

namespace FullStackCore.WebApi.Filters;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var user = context.GetHttpContext().User;
        if(user == null || user.Identity == null)
            return false;

        return user.Identity.IsAuthenticated && user.IsInRole("Admin");
    }
}

