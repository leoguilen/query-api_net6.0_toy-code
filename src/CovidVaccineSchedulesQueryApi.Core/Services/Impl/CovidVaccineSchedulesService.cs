namespace CovidVaccineSchedulesQueryApi.Core.Services;
using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using CovidVaccineSchedulesQueryApi.Core.Configurations;
using CovidVaccineSchedulesQueryApi.Core.Models;

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

        var cachedVaccineSchedule = await _cacheManager
            .GetAsync<CovidVaccineScheduleResponse>(cacheKey);

        if (cachedVaccineSchedule is not null)
        {
            var remainingCacheTime = await _cacheManager
                .KeyTimeToLiveAsync(cacheKey);

            if (remainingCacheTime.HasValue
                && remainingCacheTime >= _renewCacheTime)
            {
                return cachedVaccineSchedule;
            }
        }

        var vaccineSchedule = await _schedulesRepository.GetByAsync(personId);
        if (vaccineSchedule is null)
        {
            return default;
        }

        await _cacheManager.AddAsync(cacheKey, vaccineSchedule, _expireCacheTime);

        return vaccineSchedule;
    }

    public async ValueTask<IReadOnlyCollection<CovidVaccineScheduleResponse>> GetSchedulesAsync(DateOnly startDate, DateOnly endDate)
    {
        var cacheKey = $"key-{startDate:yyyyMMdd}-{endDate:yyyyMMdd}";

        var cachedListOfVaccineSchedules = await _cacheManager
            .GetAsync<IReadOnlyCollection<CovidVaccineScheduleResponse>>(cacheKey);

        if (cachedListOfVaccineSchedules is not null)
        {
            var remainingCacheTime = await _cacheManager
                .KeyTimeToLiveAsync(cacheKey);

            if (remainingCacheTime.HasValue
                && remainingCacheTime >= _renewCacheTime)
            {
                return cachedListOfVaccineSchedules;
            }
        }

        var listOfVaccineSchedules = await _schedulesRepository.GetAllAsync(startDate, endDate);
        if (listOfVaccineSchedules is null)
        {
            return default;
        }

        await _cacheManager.AddAsync(cacheKey, listOfVaccineSchedules, _expireCacheTime);

        return listOfVaccineSchedules;
    }
}
