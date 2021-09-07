namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Repositories;

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

    public async ValueTask<IReadOnlyList<CovidVaccineScheduleResponse>> GetAllAsync(DateTime startDate, DateTime endDate)
    {
        var filterDefinition = startDate == DateTime.MinValue
                               || endDate == DateTime.MinValue
            ? Builders<ScheduleDocument>.Filter.Empty
            : Builders<ScheduleDocument>.Filter.Between("PERSON_VACCINE_SCHEDULES.LOCAL_DATE", startDate, endDate);

        var vaccineSchedules = await (await _schedulesCollection
            .FindAsync(filterDefinition))
            .ToListAsync();

        return CovidVaccineScheduleResponseFactory
            .CreateFrom(vaccineSchedules);
    }

    public async ValueTask<CovidVaccineScheduleResponse> GetByAsync(Guid personId)
    {
        var vaccineSchedule = await (await _schedulesCollection
            .FindAsync(Builders<ScheduleDocument>
                .Filter
                .EqCaseInsensitive("PERSON_UUID", personId)))
                .FirstOrDefaultAsync();

        return CovidVaccineScheduleResponseFactory
            .CreateFrom(vaccineSchedule);
    }
}
