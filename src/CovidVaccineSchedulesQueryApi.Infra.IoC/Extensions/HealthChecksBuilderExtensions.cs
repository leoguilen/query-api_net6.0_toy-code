namespace CovidVaccineSchedulesQueryApi.Infra.IoC.Extensions;

[ExcludeFromCodeCoverage]
internal static class HealthChecksBuilderExtensions
{
    public static IServiceCollection AddChecks(this IHealthChecksBuilder healthChecksBuilder) =>
        healthChecksBuilder.Services;
}
