namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Factories;

using System;
using System.Linq;
using CovidVaccineSchedulesQueryApi.Core.Models;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Documents;

internal static class CovidVaccineScheduleResponseFactory
{
    public static CovidVaccineScheduleResponse CreateFrom(ScheduleDocument scheduleDocument) => new()
    {
        PersonId = scheduleDocument.PersonId,
        PersonName = scheduleDocument.PersonName,
        PersonDocIdentifier = scheduleDocument.PersonDocIdentifier,
        VaccineSchedules = (IReadOnlyList<PersonVaccineSchedules>)scheduleDocument
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
            }),
    };

    public static IReadOnlyList<CovidVaccineScheduleResponse> CreateFrom(IEnumerable<ScheduleDocument> schedules) =>
        !schedules.Any()
            ? Array.Empty<CovidVaccineScheduleResponse>()
            : (IReadOnlyList<CovidVaccineScheduleResponse>)schedules
                .Select(sc => new CovidVaccineScheduleResponse()
                {
                    PersonId = sc.PersonId,
                    PersonName = sc.PersonName,
                    PersonDocIdentifier = sc.PersonDocIdentifier,
                    VaccineSchedules = (IReadOnlyList<PersonVaccineSchedules>)sc.PersonVaccineSchedules
                        .Select(psc => new PersonVaccineSchedules()
                        {
                            DoseNumber = psc.DoseNumber,
                            LocalDate = psc.LocalDate,
                            LotIdentifier = psc.LotIdentifier,
                            ManufacturerName = psc.ManufacturerName,
                            ServiceUnit = psc.ServiceUnit,
                            VaccinatorName = psc.VaccinatorName,
                            WasApplied = psc.WasApplied,
                        }),
                });
}
