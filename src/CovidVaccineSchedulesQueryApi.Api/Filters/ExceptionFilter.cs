namespace CovidVaccineSchedulesQueryApi.Api.Filters;

using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

internal class ExceptionFilter : IExceptionFilter
{
    private readonly ILogWriter _logWriter;

    public ExceptionFilter(ILogWriter logWriter) =>
        _logWriter = logWriter;

    public void OnException(ExceptionContext context)
    {
        var ex = context.Exception;
        var res = new
        {
            Title = "Unexpected error",
            Description = ex.Message,
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        _logWriter.Error(
            message: ex.Message,
            ex: ex);

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(res);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
}
