namespace CovidVaccineSchedulesQueryApi.Api.Configurations;

[ExcludeFromCodeCoverage]
internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
    private readonly IConfiguration _configuration;

    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider apiVersionDescriptionProvider,
        IConfiguration configuration)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        _configuration = configuration;
    }

    public void Configure(SwaggerGenOptions options)
    {
        _apiVersionDescriptionProvider
            .ApiVersionDescriptions
            .ToList()
            .ForEach(desc =>
            {
                var info = new OpenApiInfo
                {
                    Title = "CovidVaccineSchedules.QueryApi",
                    Description = _configuration["Swagger:Description"],
                    Version = desc.ApiVersion.ToString(),
                    Contact = new()
                    {
                        Url = new UriBuilder(_configuration["Swagger:RepositoryUrl"]).Uri,
                    },
                };

                options.SwaggerDoc(desc.GroupName, info);
            });

        options.ExampleFilters();
    }
}
