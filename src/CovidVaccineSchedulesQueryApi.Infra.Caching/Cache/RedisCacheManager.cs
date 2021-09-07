namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Cache;

internal class RedisCacheManager : IAsyncCacheManager
{
    private readonly IDatabase _database;

    public RedisCacheManager(IDatabase database) =>
        _database = database;

    public async ValueTask AddAsync<T>(string key, T genericObject, TimeSpan? expiry = null) =>
        await AddCacheWithExpirationAsync(key, genericObject, expiry)
            .ConfigureAwait(false);

    public async ValueTask<bool> DeleteAsync(string key) =>
        await _database.KeyDeleteAsync(key, CommandFlags.FireAndForget);

    public async ValueTask<T> GetAsync<T>(string key) => await (
        await _database
            .SetMembersAsync(key))
            .SingleOrDefault().As<T>();

    public async ValueTask<T> GetListAsync<T>(string key) => await (
        await _database
            .SetMembersAsync(key))
            .As<T>();

    public async ValueTask<TimeSpan?> KeyTimeToLiveAsync(string key) =>
        await _database.KeyTimeToLiveAsync(key);

    private async Task AddCacheWithExpirationAsync<T>(string key, T genericObject, TimeSpan? expiry)
    {
        var genericObjectAsBytes = JsonSerializer
            .SerializeToUtf8Bytes(genericObject);

        using var memoryStream = new MemoryStream(genericObjectAsBytes);

        await _database
            .SetAddAsync(
                key: key,
                value: RedisValue.CreateFrom(memoryStream),
                flags: CommandFlags.FireAndForget);
        await _database
            .KeyExpireAsync(
                key: key,
                expiry: expiry,
                flags: CommandFlags.FireAndForget);
    }
}
