namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;

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
