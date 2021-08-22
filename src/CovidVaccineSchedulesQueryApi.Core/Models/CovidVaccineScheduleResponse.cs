namespace CovidVaccineSchedulesQueryApi.Core.Models;

public record CovidVaccineScheduleResponse
{
    public Guid PersonId { get; internal init; }

    public string PersonName { get; internal init; }

    public string PersonDocIdentifier { get; internal init; }

    public IReadOnlyCollection<PersonVaccineSchedules> VaccineSchedules { get; internal init; }
}
