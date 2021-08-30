namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Extensions;

using System.Diagnostics.CodeAnalysis;
using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using CovidVaccineSchedulesQueryApi.Core.Configurations;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfraData(this IServiceCollection services) =>
        services
            .ConfigureMongoConnection()
            .AddScoped<ISchedulesRepository, SchedulesRepository>();

    public static IServiceCollection ConfigureMongoConnection(this IServiceCollection services) =>
        services
            .AddScoped(provider =>
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
                var mongoConfig = provider.GetRequiredService<MongoDbConfiguration>();
                var mongoUrl = MongoUrl.Create(mongoConfig.ConnectionString);

                var mongoClient = new MongoClient(mongoUrl);

                return mongoClient.GetDatabase(mongoUrl.DatabaseName);
            });
}
