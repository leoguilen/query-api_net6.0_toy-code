namespace CovidVaccineSchedulesQueryApi.Core.Models;

public record CovidVaccineScheduleResponse
{
    public Guid PersonId { get; init; }

    public string PersonName { get; init; }

    public string PersonDocIdentifier { get; init; }

    public IReadOnlyList<PersonVaccineSchedules> VaccineSchedules { get; init; }
}
