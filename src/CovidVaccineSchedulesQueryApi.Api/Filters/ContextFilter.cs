namespace CovidVaccineSchedulesQueryApi.Api.Filters;

using CovidVaccineSchedulesQueryApi.Api.Constants;
using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

internal class ContextFilter : IActionFilter
{
    private readonly ILogWriter _logWriter;

    public ContextFilter(ILogWriter logWriter) =>
        _logWriter = logWriter;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Not used
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var headers = context.HttpContext.Request.Headers;
        var correlationId = headers[HttpHeaders.CorrelationId];
        var correlationIdParsed = Guid.TryParse(correlationId, out var guid)
            ? guid
            : Guid.NewGuid();

        _logWriter.SetCorrelationId(correlationIdParsed);
    }
}
