namespace CovidVaccineSchedulesQueryApi.Api.Extensions;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services) =>
        services
            .AddControllers(opt =>
            {
                opt.Filters.Add<ContextFilter>();
                opt.Filters.Add<ControllersFilter>();
                opt.Filters.Add<ExceptionFilter>();
            })
            .Services
            .ConfigAppVersioning()
            .ConfigSwagger();

    private static IServiceCollection ConfigAppVersioning(this IServiceCollection services) =>
        services
            .AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new(1, 0);
                opt.ReportApiVersions = true;
            })
            .AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
            });

    private static IServiceCollection ConfigSwagger(this IServiceCollection services) =>
        services
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
            .AddSwaggerGen(opt =>
            {
                opt.OperationFilter<SwaggerDefaultValues>();

                var xmlApiFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlApiPath = Path.Combine(AppContext.BaseDirectory, xmlApiFile);

                opt.ExampleFilters();
                opt.CustomSchemaIds(type => type.FullName);
                opt.IncludeXmlComments(xmlApiPath);
            })
            .AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
}
