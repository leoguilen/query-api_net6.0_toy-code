namespace CovidVaccineSchedulesQueryApi.Core.Extensions;

using System.Diagnostics.CodeAnalysis;
using CovidVaccineSchedulesQueryApi.Core.Configurations;
using CovidVaccineSchedulesQueryApi.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
.ApplyConfigurations(configuration)
.AddScoped<ICovidVaccineSchedulesService, CovidVaccineSchedulesService>();
    }

    private static IServiceCollection ApplyConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
.AddSingleton<MongoDbConfiguration>(_ => new()
{
    ConnectionString = configuration.GetConnectionString("MongoDb"),
})
.AddSingleton<RedisConfiguration>(_ => new()
{
    ConnectionString = configuration.GetConnectionString("Redis"),
    TimeToExpireInSeconds = int.Parse(configuration["Cache:TimeToExpireInSeconds"]),
    TimeToRenewInSeconds = int.Parse(configuration["Cache:TimeToRenewInSeconds"]),
})
.AddSingleton<VaultConfiguration>(_ => new()
{
    UseVault = bool.Parse(configuration["Vault:UseVault"]),
    ClientId = configuration["Vault:ClientId"],
    ClientSecret = configuration["Vault:ClientSecret"],
    Endpoint = configuration["Vault:Endpoint"],
});
    }
}
