using CovidVaccineSchedulesQueryApi.Api.Configurations;
using CovidVaccineSchedulesQueryApi.Api.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.IoC.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApi()
    .AddIoC(builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.ConfigureSwagger(builder.Configuration, provider);
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

await app.RunAsync();
