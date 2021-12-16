namespace CovidVaccineSchedulesQueryApi.Core.Services;

internal class CovidVaccineSchedulesService : ICovidVaccineSchedulesService
{
    private readonly ISchedulesRepository _schedulesRepository;
    private readonly IAsyncCacheManager _cacheManager;
    private readonly TimeSpan _expireCacheTime;
    private readonly TimeSpan _renewCacheTime;

    public CovidVaccineSchedulesService(
        ISchedulesRepository schedulesRepository,
        IAsyncCacheManager cacheManager,
        RedisConfiguration cacheConfig)
    {
        _schedulesRepository = schedulesRepository;
        _cacheManager = cacheManager;
        _expireCacheTime = TimeSpan.FromSeconds(cacheConfig.TimeToExpireInSeconds);
        _renewCacheTime = TimeSpan.FromSeconds(cacheConfig.TimeToRenewInSeconds);
    }

    public async ValueTask<CovidVaccineScheduleResponse> GetScheduleByAsync(Guid personId)
    {
        var cacheKey = $"key-{personId}";

        var cacheValue = await GetCachedValueAsync<CovidVaccineScheduleResponse>(cacheKey);
        if (cacheValue is not null)
        {
            return cacheValue;
        }

        var vaccineSchedule = await _schedulesRepository
            .GetByAsync(personId);

        return await AddToCacheAndReturnAsync(cacheKey, vaccineSchedule);
    }

    public async ValueTask<IReadOnlyList<CovidVaccineScheduleResponse>> GetSchedulesAsync(DateTime startDate, DateTime endDate)
    {
        var cacheKey = $"key-{startDate:yyyyMMdd}-{endDate:yyyyMMdd}";

        var cacheValue = await GetCachedValueAsync<IReadOnlyList<CovidVaccineScheduleResponse>>(cacheKey);
        if (cacheValue is not null)
        {
            return cacheValue;
        }

        var listOfVaccineSchedules = await _schedulesRepository
            .GetAllAsync(startDate, endDate);

        return await AddToCacheAndReturnAsync(cacheKey, listOfVaccineSchedules);
    }

    private async ValueTask<T> GetCachedValueAsync<T>(string cacheKey)
    {
        var cachedValue = typeof(T).IsIEnumerableOf()
            ? await _cacheManager
                .GetListAsync<T>(cacheKey)
            : await _cacheManager
                .GetAsync<T>(cacheKey);

        if (cachedValue is null)
        {
            return default;
        }

        var remainingCacheTime = await _cacheManager
                .KeyTimeToLiveAsync(cacheKey);

        return (remainingCacheTime.HasValue
            && remainingCacheTime >= _renewCacheTime)
            ? cachedValue
            : default;
    }

    private async ValueTask<T> AddToCacheAndReturnAsync<T>(string cacheKey, T value)
    {
        if (value is not null)
        {
            await _cacheManager
                .AddAsync(cacheKey, value, _expireCacheTime);
            return value;
        }

        return default;
    }
}
