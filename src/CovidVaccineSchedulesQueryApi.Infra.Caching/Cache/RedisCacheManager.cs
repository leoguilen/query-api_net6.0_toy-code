namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Cache;

using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;
using StackExchange.Redis;

internal class RedisCacheManager : IAsyncCacheManager
{
    private readonly IDatabaseAsync _database;

    public RedisCacheManager(IDatabaseAsync database) =>
        _database = database;

    public async ValueTask AddAsync<T>(string key, T obj, TimeSpan? expiry = null) =>
        await Task.WhenAll(
            Task.Run(async () => await _database.HashSetAsync(key, obj.ToHashEntries(), CommandFlags.FireAndForget)),
            Task.Run(async () => await _database.KeyExpireAsync(key, expiry, CommandFlags.FireAndForget)));

    public async ValueTask<bool> DeleteAsync(string key) =>
        await _database.KeyDeleteAsync(key, CommandFlags.FireAndForget);

    public async ValueTask<T> GetAsync<T>(string key)
    {
        var cachedData = await _database.HashGetAllAsync(key);
        return cachedData?.Length > 0
            ? cachedData.ConvertTo<T>()
            : default;
    }

    public async ValueTask<TimeSpan?> KeyTimeToLiveAsync(string key) =>
        await _database.KeyTimeToLiveAsync(key);
}
