namespace CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;

using CovidVaccineSchedulesQueryApi.Core.Models;

public interface ISchedulesRepository
{
    ValueTask<IReadOnlyList<CovidVaccineScheduleResponse>> GetAllAsync(DateOnly startDate, DateOnly endDate);

    ValueTask<CovidVaccineScheduleResponse> GetByAsync(Guid personId);
}
