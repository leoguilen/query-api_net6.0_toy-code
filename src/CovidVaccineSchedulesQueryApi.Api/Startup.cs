namespace CovidVaccineSchedulesQueryApi.Api;

[ExcludeFromCodeCoverage]
public class Startup
{
    public Startup(IConfiguration configuration) =>
        Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) => services
        .AddApi()
        .AddIoC(Configuration);

    public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider) => app
        .UseApi(Configuration, provider);
}
