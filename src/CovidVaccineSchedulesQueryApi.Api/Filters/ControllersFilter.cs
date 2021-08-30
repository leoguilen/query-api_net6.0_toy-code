namespace CovidVaccineSchedulesQueryApi.Api.Filters;

using System.Diagnostics;
using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

internal class ControllersFilter : IActionFilter
{
    private readonly ILogWriter _logWriter;

    private Stopwatch _stopwatch;

    public ControllersFilter(ILogWriter logWriter) =>
        _logWriter = logWriter;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();

        var controllerName = context.Controller.GetType().FullName;
        _logWriter.Info($"Executed {controllerName} in {_stopwatch.ElapsedMilliseconds} ms");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();

        var controllerName = context.Controller.GetType().FullName;
        _logWriter.Info($"Executing {controllerName}");
    }
}
