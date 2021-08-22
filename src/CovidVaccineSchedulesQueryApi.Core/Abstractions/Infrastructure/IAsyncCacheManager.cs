namespace CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;

public interface IAsyncCacheManager
{
    ValueTask<bool> AddAsync<T>(string key, T obj, TimeSpan? expiry = null);

    ValueTask<T> GetAsync<T>(string key);

    ValueTask<bool> DeleteAsync(string key);

    ValueTask<TimeSpan?> KeyTimeToLiveAsync(string key);
}
