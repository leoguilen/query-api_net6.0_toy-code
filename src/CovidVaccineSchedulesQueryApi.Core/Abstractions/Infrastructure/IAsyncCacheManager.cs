namespace CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;

public interface IAsyncCacheManager
{
    ValueTask AddAsync<T>(string key, T genericObject, TimeSpan? expiry = null);

    ValueTask<T> GetAsync<T>(string key);

    ValueTask<T> GetListAsync<T>(string key);

    ValueTask<bool> DeleteAsync(string key);

    ValueTask<TimeSpan?> KeyTimeToLiveAsync(string key);
}
