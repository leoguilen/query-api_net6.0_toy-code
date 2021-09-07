namespace CovidVaccineSchedulesQueryApi.Api.Configurations;

[ExcludeFromCodeCoverage]
internal static class SwaggerExtensions
{
    public static IApplicationBuilder ConfigureSwagger(
        this IApplicationBuilder app,
        IConfiguration configuration,
        IApiVersionDescriptionProvider provider)
    {
        var swaggerEnabled = configuration
            .GetValue("Swagger:Enabled", false);

        return !swaggerEnabled
            ? app
            : app
            .UseSwagger()
            .UseSwaggerUI(opt => provider
                .ApiVersionDescriptions
                .ToList()
                .ForEach(desc => opt
                    .SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpper())));
    }
}
