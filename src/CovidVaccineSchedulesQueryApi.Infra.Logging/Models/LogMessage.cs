namespace CovidVaccineSchedulesQueryApi.Infra.Logging.Models;

internal record LogMessage
{
    public DateTime Timestamp { get; internal init; }

    public string Application { get; internal init; }

    public string Version { get; internal init; }

    public string Level { get; internal init; }

    public string Message { get; internal init; }

    public string CorrelationId { get; internal init; }

    public object Data { get; internal init; }

    public string Method { get; internal init; }

    public string File { get; internal init; }

    public string StackTrace { get; internal init; }

    public Exception? Exception { get; internal init; }
}
