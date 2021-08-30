namespace CovidVaccineSchedulesQueryApi.Infra.IoC.DependencyInjection;

using System.Diagnostics.CodeAnalysis;
using CovidVaccineSchedulesQueryApi.Core.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.IoC.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.Logging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddChecks();
}
