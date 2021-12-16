namespace CovidVaccineSchedulesQueryApi.Core.Services;

public interface ICovidVaccineSchedulesService
{
    ValueTask<IReadOnlyList<CovidVaccineScheduleResponse>> GetSchedulesAsync(DateTime startDate, DateTime endDate);

    ValueTask<CovidVaccineScheduleResponse> GetScheduleByAsync(Guid personId);
}
