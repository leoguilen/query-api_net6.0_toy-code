namespace CovidVaccineSchedulesQueryApi.Core.Services;

using CovidVaccineSchedulesQueryApi.Core.Models;

public interface ICovidVaccineSchedulesService
{
    ValueTask<IReadOnlyCollection<CovidVaccineScheduleResponse>> GetSchedulesAsync(DateOnly startDate, DateOnly endDate);

    ValueTask<CovidVaccineScheduleResponse> GetScheduleByAsync(Guid personId);
}
