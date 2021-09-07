namespace CovidVaccineSchedulesQueryApi.Api.Extensions;

[ExcludeFromCodeCoverage]
internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApi(this WebApplication app, IConfiguration configuration)
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.ConfigureSwagger(configuration, provider);
        app.UseRouting();
        app.MapControllers();
        app.MapHealthChecks("/healthcheck", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
            AllowCachingResponses = false,
        });

        return app;
    }
}
