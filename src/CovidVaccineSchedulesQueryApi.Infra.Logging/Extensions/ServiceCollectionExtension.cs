namespace CovidVaccineSchedulesQueryApi.Infra.Logging.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddLogging(this IServiceCollection services) =>
        services
            .ConfigLogger()
            .AddScoped<ILogWriter, SerilogWriter>();

    private static IServiceCollection ConfigLogger(this IServiceCollection services) =>
        services
            .AddSingleton(_ =>
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}")
                    .CreateLogger();

                return Log.Logger;
            });
}
