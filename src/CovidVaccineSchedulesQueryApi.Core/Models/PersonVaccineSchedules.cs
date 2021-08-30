namespace CovidVaccineSchedulesQueryApi.Core.Models;

public record PersonVaccineSchedules
{
    public byte DoseNumber { get; init; }

    public string ServiceUnit { get; init; }

    public DateTimeOffset LocalDate { get; init; }

    public string LotIdentifier { get; init; }

    public string ManufacturerName { get; init; }

    public string VaccinatorName { get; init; }

    public bool WasApplied { get; init; }
}
