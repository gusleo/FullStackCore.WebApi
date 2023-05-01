using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FullStackCore.WebApi.Filters;

public class ErrorHandlingFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger _logger;

    public ErrorHandlingFilter(ILogger<ErrorHandlingFilter> logger)
    {
        _logger = logger;
    }

    public int Order { get; set; } = int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            _logger.LogError(context.Exception, "An error occurred while processing the request.");
        }
    }
}