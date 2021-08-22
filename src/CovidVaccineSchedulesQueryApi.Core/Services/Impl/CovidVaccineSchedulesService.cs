namespace CovidVaccineSchedulesQueryApi.Core.Services;

internal class CovidVaccineSchedulesService : ICovidVaccineSchedulesService
{
    public ValueTask<byte> GetScheduleByAsync(Guid personId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<byte[]> GetSchedulesAsync(DateOnly startDate, DateOnly endDate)
    {
        throw new NotImplementedException();
    }
}
