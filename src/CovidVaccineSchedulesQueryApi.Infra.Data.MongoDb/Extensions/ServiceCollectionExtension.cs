namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtension
{
    private const string DatabaseName = "VACCINE_SCHEDULES";

    public static IServiceCollection AddInfraData(this IServiceCollection services) =>
        services
            .ConfigureMongoConnection()
            .AddScoped<ISchedulesRepository, SchedulesRepository>();

    public static IServiceCollection ConfigureMongoConnection(this IServiceCollection services) =>
        services
            .AddScoped(provider =>
            {
                var mongoConfig = provider.GetRequiredService<MongoDbConfiguration>();
                var mongoClient = new MongoClient(mongoConfig.ConnectionString);

                return mongoClient.GetDatabase(DatabaseName);
            });
}
