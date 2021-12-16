using System.Diagnostics;
using Xunit.Abstractions;

namespace CovidVaccineSchedulesQueryApi.IntegrationTest.Controllers;

[Trait("IntegrationTestPerfomance", nameof(SchedulesController))]
public class SchedulesControllerPerformanceTest : ControllerTestsBase
{
    private const string ExistentFirstPersonId = "2f668b10-127e-11ec-82a8-0242ac130003";
    private const string ExistentSecondPersonId = "34311944-127e-11ec-82a8-0242ac130003";

    private readonly ITestOutputHelper _outputHelper;

    public SchedulesControllerPerformanceTest(
        WebApplicationFactoryTest factoryTest,
        ITestOutputHelper outputHelper)
        : base(factoryTest)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task GetPersonVaccineScheduleAsync_GivenCalledWithoutValueCached_ThenExecutionTimeMustBeLessOrEqualTo400MS()
    {
        // Arrange
        const int MaxExecutionTimeInMilisecondsAccepted = 400;
        var personId = Guid.Parse(ExistentFirstPersonId);
        var executionTimer = Stopwatch.StartNew();

        // Act
        _ = await Client.GetAsync($"{DefaultControllerRoute}/{personId}");

        // Assert
        executionTimer.Stop();
        _outputHelper.WriteLine($"Execution took {executionTimer.ElapsedMilliseconds} ms");
        executionTimer.ElapsedMilliseconds
            .Should()
            .BeLessOrEqualTo(MaxExecutionTimeInMilisecondsAccepted);
    }

    [Fact]
    public async Task GetPersonVaccineScheduleAsync_GivenCalledWithValueCached_ThenExecutionTimeMustBeLessOrEqualTo200MS()
    {
        // Arrange
        const int MaxExecutionTimeInMilisecondsAccepted = 200;
        var personId = Guid.Parse(ExistentSecondPersonId);
        await CallEndpointToAddValueToCacheAsync(personId);
        var executionTimer = Stopwatch.StartNew();

        // Act
        _ = await Client.GetAsync($"{DefaultControllerRoute}/{personId}");

        // Assert
        executionTimer.Stop();
        _outputHelper.WriteLine($"Execution took {executionTimer.ElapsedMilliseconds} ms");
        executionTimer.ElapsedMilliseconds
            .Should()
            .BeLessOrEqualTo(MaxExecutionTimeInMilisecondsAccepted);
    }

    [Fact]
    public async Task GetAllVaccineSchedulesAsync_GivenCalledWithoutValueCached_ThenExecutionTimeMustBeLessOrEqualTo400MS()
    {
        // Arrange
        const int MaxExecutionTimeInMilisecondsAccepted = 400;
        var startDate = new DateTime(2021, 3, 27, 8, 30, 15).ToString("yyyy-MM-dd");
        var endDate = DateTime.Now.ToString("yyyy-MM-dd");
        var executionTimer = Stopwatch.StartNew();

        // Act
        _ = await Client.GetAsync($"{DefaultControllerRoute}?startDate={startDate}&endDate={endDate}");

        // Assert
        executionTimer.Stop();
        _outputHelper.WriteLine($"Execution took {executionTimer.ElapsedMilliseconds} ms");
        executionTimer.ElapsedMilliseconds
            .Should()
            .BeLessOrEqualTo(MaxExecutionTimeInMilisecondsAccepted);
    }

    private async Task CallEndpointToAddValueToCacheAsync(Guid personId) =>
        _ = await Client.GetAsync($"{DefaultControllerRoute}/{personId}");
}
