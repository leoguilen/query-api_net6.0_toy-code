namespace CovidVaccineSchedulesQueryApi.IntegrationTest.Controllers;

[Trait("IntegrationTest", nameof(SchedulesController))]
public class SchedulesControllerTest : ControllerTestsBase
{
    private const string ExistentFirstPersonId = "2f668b10-127e-11ec-82a8-0242ac130003";
    private const string ExistentSecondPersonId = "34311944-127e-11ec-82a8-0242ac130003";

    public SchedulesControllerTest(WebApplicationFactoryTest factoryTest)
        : base(factoryTest)
    {
    }

    [Fact]
    public async Task GetPersonVaccineScheduleAsync_GivenExistentRegisterWithPersonIdSpecified_ThenResponseWithStatusOkAndValueRetrieved()
    {
        // Arrange
        var personId = Guid.Parse(ExistentFirstPersonId);

        // Act
        var response = await Client
            .GetAsync($"{DefaultControllerRoute}/{personId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response
            .Content
            .ReadFromJsonAsync<CovidVaccineScheduleResponse>())
        .Should()
        .Match<CovidVaccineScheduleResponse>(response => response.PersonId == personId);
    }

    [Fact]
    public async Task GetPersonVaccineScheduleAsync_GivenNonexistentRegisterWithPersonIdSpecified_ThenResponseWithStatusNotFound()
    {
        // Arrange
        var nonexistentPersonId = Guid.NewGuid();

        // Act
        var response = await Client
            .GetAsync($"{DefaultControllerRoute}/{nonexistentPersonId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPersonVaccineScheduleAsync_GivenExistentValueInCacheWithCacheKey_ThenResponseWithStatusOkAndCachedValueRetrieved()
    {
        // Arrange
        var personId = Guid.Parse(ExistentSecondPersonId);
        await CallEndpointToAddValueToCacheAsync(personId);

        // Act
        var response = await Client
            .GetAsync($"{DefaultControllerRoute}/{personId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response
            .Content
            .ReadFromJsonAsync<CovidVaccineScheduleResponse>())
        .Should()
        .Match<CovidVaccineScheduleResponse>(response => response.PersonId == personId);
    }

    [Fact]
    public async Task GetAllVaccineSchedulesAsync_GivenExistentRegistersBetweenDateRangeSpecified_ThenReturnResponseWithStatusOkAndValuesRetrieved()
    {
        // Arrange
        var startDate = new DateTime(2021, 3, 27, 8, 30, 15).ToString("yyyy-MM-dd");
        var endDate = DateTime.Now.ToString("yyyy-MM-dd");

        // Act
        var response = await Client
            .GetAsync($"{DefaultControllerRoute}?startDate={startDate}&endDate={endDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response
            .Content
            .ReadFromJsonAsync<CovidVaccineScheduleResponse[]>())
        .Should()
        .SatisfyRespectively(
            res1 => res1.PersonId.Should().Be(ExistentFirstPersonId),
            res2 => res2.PersonId.Should().Be(ExistentSecondPersonId));
    }

    [Fact]
    public async Task GetAllVaccineSchedulesAsync_GivenNonexistentRegistersBetweenDateRangeSpecified_ThenReturnResponseWithStatusNotFound()
    {
        // Arrange
        var startDate = DateTime.Now.ToString("yyyy-MM-dd");
        var endDate = DateTime.Now.ToString("yyyy-MM-dd");

        // Act
        var response = await Client
            .GetAsync($"{DefaultControllerRoute}?startDate={startDate}&endDate={endDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task CallEndpointToAddValueToCacheAsync(Guid personId) =>
        _ = await Client.GetAsync($"{DefaultControllerRoute}/{personId}");
}
