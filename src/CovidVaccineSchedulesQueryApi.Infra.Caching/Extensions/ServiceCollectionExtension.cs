namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;

using System.Diagnostics.CodeAnalysis;
using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using CovidVaccineSchedulesQueryApi.Core.Configurations;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Cache;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfraCaching(this IServiceCollection services) =>
        services
            .AddScoped(provider =>
            {
                var redisConnectionString = provider
                    .GetRequiredService<RedisConfiguration>()
                    .ConnectionString;

                return ConnectionMultiplexer
                    .Connect(redisConnectionString)
                    .GetDatabase();
            })
            .AddScoped<IAsyncCacheManager, RedisCacheManager>();
}
