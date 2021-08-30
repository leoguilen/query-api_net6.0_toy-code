namespace CovidVaccineSchedulesQueryApi.Infra.Logging.Logging;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using CovidVaccineSchedulesQueryApi.Infra.Logging.Models;
using Serilog;
using Serilog.Events;

internal class SerilogWriter : ILogWriter
{
    private ILogger _logger;
    private Guid _correlationId;
    private bool _disposed;

    public SerilogWriter(ILogger logger) =>
        _logger = logger;

    public void SetCorrelationId(Guid correlationId) =>
        _correlationId = correlationId;

    public void Error(
        string message,
        object? data = null,
        Exception? ex = null,
        [CallerMemberName] string sourceMethod = "",
        [CallerFilePath] string sourceFile = "")
    {
        Log(message, data, sourceMethod, sourceFile, LogEventLevel.Error, ex, _logger.Error);
    }

    public void Fatal(
        string message,
        object? data = null,
        Exception? ex = null,
        [CallerMemberName] string sourceMethod = "",
        [CallerFilePath] string sourceFile = "")
    {
        Log(message, data, sourceMethod, sourceFile, LogEventLevel.Fatal, ex, _logger.Fatal);
    }

    public void Info(
        string message,
        object? data = null,
        Exception? ex = null,
        [CallerMemberName] string sourceMethod = "",
        [CallerFilePath] string sourceFile = "")
    {
        Log(message, data, sourceMethod, sourceFile, LogEventLevel.Information, ex, _logger.Information);
    }

    public void Warn(
        string message,
        object? data = null,
        Exception? ex = null,
        [CallerMemberName] string sourceMethod = "",
        [CallerFilePath] string sourceFile = "")
    {
        Log(message, data, sourceMethod, sourceFile, LogEventLevel.Warning, ex, _logger.Warning);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _logger = null;
            }

            _disposed = true;
        }
    }

    private static (string? Name, string? Version) GetAppInfo()
    {
        var entryAssembly = Assembly.GetEntryAssembly();

        return (
            Name: entryAssembly?
                .GetName()?
                .Name,
            Version: entryAssembly?
                .GetName()?
                .Version?
                .ToString());
    }

    private void Log(
        string message,
        object data,
        string sourceMethod,
        string sourceFile,
        LogEventLevel level,
        Exception? ex,
        Action<string, LogMessage> logger)
    {
        var appInfo = GetAppInfo();
        var correlationId = _correlationId == Guid.Empty ? Guid.NewGuid() : _correlationId;

        var logMessage = new LogMessage
        {
            Timestamp = DateTime.Now,
            Application = appInfo.Name ?? string.Empty,
            Version = appInfo.Version ?? string.Empty,
            CorrelationId = correlationId.ToString(),
            Data = data,
            Level = level.ToString(),
            Method = sourceMethod,
            File = sourceFile,
            Message = message,
            Exception = ex,
            StackTrace = $"{ex?.Message} {ex?.StackTrace}",
        };

        logger.Invoke("{@LogMessage}", logMessage);
    }
}
