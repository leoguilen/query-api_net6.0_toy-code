namespace CovidVaccineSchedulesQueryApi.Infra.IoC.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIoC(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddCore(configuration)
            .AddInfraCaching()
            .AddInfraData()
            .AddLogging()
            .AddHealthChecks()
                .AddChecks(configuration);
}
