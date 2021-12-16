namespace CovidVaccineSchedulesQueryApi.Infra.IoC.Extensions;

[ExcludeFromCodeCoverage]
internal static class HealthChecksBuilderExtensions
{
    public static IServiceCollection AddChecks(
        this IHealthChecksBuilder healthChecksBuilder,
        IConfiguration configuration) =>
        healthChecksBuilder
            .AddMongoDb(
                mongodbConnectionString: configuration.GetConnectionString("MongoDb"),
                name: "MongoDb",
                failureStatus: HealthStatus.Unhealthy)
            .AddRedis(
                redisConnectionString: configuration.GetConnectionString("Redis"),
                name: "Redis",
                failureStatus: HealthStatus.Unhealthy)
            .Services;
}
