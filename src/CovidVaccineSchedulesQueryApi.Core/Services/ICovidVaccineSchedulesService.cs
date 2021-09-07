namespace CovidVaccineSchedulesQueryApi.Core.Services;

using CovidVaccineSchedulesQueryApi.Core.Models;

public interface ICovidVaccineSchedulesService
{
    ValueTask<IReadOnlyList<CovidVaccineScheduleResponse>> GetSchedulesAsync(DateTime startDate, DateTime endDate);

    ValueTask<CovidVaccineScheduleResponse> GetScheduleByAsync(Guid personId);
}
