namespace CovidVaccineSchedulesQueryApi.Infra.IoC.Extensions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class HealthChecksBuilderExtensions
{
    public static IServiceCollection AddChecks(this IHealthChecksBuilder healthChecksBuilder) =>
        healthChecksBuilder.Services;
}
