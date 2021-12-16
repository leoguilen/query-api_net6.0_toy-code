namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Factories;

internal static class CovidVaccineScheduleResponseFactory
{
    public static CovidVaccineScheduleResponse CreateFrom(ScheduleDocument scheduleDocument) =>
        scheduleDocument is null
            ? default
            : new()
            {
                PersonId = scheduleDocument.PersonId,
                PersonName = scheduleDocument.PersonName,
                PersonDocIdentifier = scheduleDocument.PersonDocIdentifier,
                VaccineSchedules = scheduleDocument
                    .PersonVaccineSchedules
                    .Select(sc => new PersonVaccineSchedules()
                    {
                        DoseNumber = sc.DoseNumber,
                        LocalDate = sc.LocalDate,
                        LotIdentifier = sc.LotIdentifier,
                        ManufacturerName = sc.ManufacturerName,
                        ServiceUnit = sc.ServiceUnit,
                        VaccinatorName = sc.VaccinatorName,
                        WasApplied = sc.WasApplied,
                    })
                    .ToArray(),
            };

    public static IReadOnlyList<CovidVaccineScheduleResponse> CreateFrom(IEnumerable<ScheduleDocument> schedules) =>
        schedules?.Any() != true
            ? Array.Empty<CovidVaccineScheduleResponse>()
            : schedules
                .Select(sc => new CovidVaccineScheduleResponse()
                {
                    PersonId = sc.PersonId,
                    PersonName = sc.PersonName,
                    PersonDocIdentifier = sc.PersonDocIdentifier,
                    VaccineSchedules = sc.PersonVaccineSchedules
                        .Select(psc => new PersonVaccineSchedules()
                        {
                            DoseNumber = psc.DoseNumber,
                            LocalDate = psc.LocalDate,
                            LotIdentifier = psc.LotIdentifier,
                            ManufacturerName = psc.ManufacturerName,
                            ServiceUnit = psc.ServiceUnit,
                            VaccinatorName = psc.VaccinatorName,
                            WasApplied = psc.WasApplied,
                        })
                        .ToArray(),
                })
                .ToArray();
}
