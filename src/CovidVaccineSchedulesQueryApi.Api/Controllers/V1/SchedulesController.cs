namespace CovidVaccineSchedulesQueryApi.Api.Controllers.V1;

using CovidVaccineSchedulesQueryApi.Api.Constants;
using CovidVaccineSchedulesQueryApi.Core.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(HttpHeaders.ContentType)]
[Consumes(HttpHeaders.ContentType)]
public class SchedulesController : ControllerBase
{
    private readonly ICovidVaccineSchedulesService _vaccineSchedulesService;

    public SchedulesController(ICovidVaccineSchedulesService vaccineSchedulesService) =>
        _vaccineSchedulesService = vaccineSchedulesService;

    /// <summary>
    /// Realiza a busca do agendamento de vacina contra covid de uma pessoa.
    /// </summary>
    /// <param name="personId">Identificador único de uma pessoa.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not Found.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <response code="503">Service Unavailable.</response>
    /// <returns>Retorna os detalhes dos agendamentos dessa pessoa.</returns>
    [HttpGet("{personId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetPersonVaccineScheduleAsync([FromRoute] Guid personId)
    {
        var scheduleResponse = await _vaccineSchedulesService
            .GetScheduleByAsync(personId);

        return scheduleResponse is null
            ? NotFound()
            : Ok(scheduleResponse);
    }

    /// <summary>
    /// Realiza a busca dos agendamentos de vacina contra covid dentro de um intervalo de tempo.
    /// </summary>
    /// <param name="startDate">Data inicial.</param>
    /// <param name="endDate">Data final.</param>
    /// <response code="200">OK.</response>
    /// <response code="404">Not Found.</response>
    /// <response code="412">Precondition Failed.</response>
    /// <response code="500">Internal Server Error.</response>
    /// <response code="503">Service Unavailable.</response>
    /// <returns>Retorna os detalhes de todos os agendamentos no intervalo de tempo.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetAllVaccineSchedulesAsync([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var scheduleResponse = await _vaccineSchedulesService
            .GetSchedulesAsync(startDate, endDate);

        return scheduleResponse?.Any() != true
            ? NotFound()
            : Ok(scheduleResponse);
    }
}
