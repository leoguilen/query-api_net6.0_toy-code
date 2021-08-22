namespace CovidVaccineSchedulesQueryApi.Core.Services;

public interface ICovidVaccineSchedulesService
{
    ValueTask<byte[]> GetSchedulesAsync(DateOnly startDate, DateOnly endDate);

    ValueTask<byte> GetScheduleByAsync(Guid personId);
}
