namespace CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;

public interface ISchedulesRepository
{
    ValueTask<IReadOnlyList<CovidVaccineScheduleResponse>> GetAllAsync(DateTime startDate, DateTime endDate);

    ValueTask<CovidVaccineScheduleResponse> GetByAsync(Guid personId);
}
