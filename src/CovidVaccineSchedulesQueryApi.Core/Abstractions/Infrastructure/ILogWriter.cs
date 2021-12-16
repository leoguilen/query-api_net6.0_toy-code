namespace CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;

public interface ILogWriter : IDisposable
{
    void SetCorrelationId(Guid correlationId);

    void Info(string message, object? data = default, Exception? ex = default, [CallerMemberName] string sourceMethod = "", [CallerFilePath] string sourceFile = "");

    void Warn(string message, object? data = default, Exception? ex = default, [CallerMemberName] string sourceMethod = "", [CallerFilePath] string sourceFile = "");

    void Error(string message, object? data = default, Exception? ex = default, [CallerMemberName] string sourceMethod = "", [CallerFilePath] string sourceFile = "");

    void Fatal(string message, object? data = default, Exception? ex = default, [CallerMemberName] string sourceMethod = "", [CallerFilePath] string sourceFile = "");
}
