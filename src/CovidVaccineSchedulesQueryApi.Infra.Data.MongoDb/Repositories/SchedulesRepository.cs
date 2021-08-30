namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Repositories;

using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using CovidVaccineSchedulesQueryApi.Core.Models;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Documents;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Factories;
using MongoDB.Driver;

internal class SchedulesRepository : ISchedulesRepository
{
    private const string CollectionName = "PERSON_VACCINE_SCHEDULES";

    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<ScheduleDocument> _schedulesCollection;

    public SchedulesRepository(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
        _schedulesCollection = _mongoDatabase
            .GetCollection<ScheduleDocument>(CollectionName);
    }

    public async ValueTask<IReadOnlyList<CovidVaccineScheduleResponse>> GetAllAsync(DateOnly startDate, DateOnly endDate)
    {
        var vaccineSchedules = await _schedulesCollection
            .FindAsync(Builders<ScheduleDocument>
                .Filter
                .Between("PERSON_VACCINE_SCHEDULES.LOCAL_DATE", startDate, endDate));

        return CovidVaccineScheduleResponseFactory
            .CreateFrom(vaccineSchedules.Current);
    }

    public async ValueTask<CovidVaccineScheduleResponse> GetByAsync(Guid personId)
    {
        var vaccineSchedule = await _schedulesCollection
            .FindAsync(Builders<ScheduleDocument>
                .Filter
                .EqCaseInsensitive("PERSON_UUID", personId));

        return CovidVaccineScheduleResponseFactory
            .CreateFrom(vaccineSchedule.Current.GetEnumerator().Current);
    }
}
