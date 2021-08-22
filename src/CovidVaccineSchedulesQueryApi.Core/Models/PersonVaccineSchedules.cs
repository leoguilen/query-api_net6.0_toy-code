namespace CovidVaccineSchedulesQueryApi.Core.Models;

public record PersonVaccineSchedules
{
    public byte DoseNumber { get; internal init; }

    public string ServiceUnit { get; internal init; }

    public DateTimeOffset LocalDate { get; internal init; }

    public string LotIdentifier { get; internal init; }

    public string ManufacturerName { get; internal init; }

    public string VaccinatorName { get; internal init; }

    public bool WasApplied { get; internal init; }
}
