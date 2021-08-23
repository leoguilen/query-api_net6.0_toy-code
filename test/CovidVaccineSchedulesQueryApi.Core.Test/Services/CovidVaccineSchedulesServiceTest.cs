namespace CovidVaccineSchedulesQueryApi.Core.Test.Services;

using AutoFixture.Xunit2;
using CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;
using CovidVaccineSchedulesQueryApi.Core.Configurations;
using CovidVaccineSchedulesQueryApi.Core.Models;
using CovidVaccineSchedulesQueryApi.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

[Trait("Unit", nameof(CovidVaccineSchedulesService))]
public class CovidVaccineSchedulesServiceTest
{
    private readonly Mock<ISchedulesRepository> _schedulesRepository;
    private readonly Mock<IAsyncCacheManager> _cacheManager;
    private readonly RedisConfiguration _cacheConfig;

    public CovidVaccineSchedulesServiceTest()
    {
        _schedulesRepository = new(MockBehavior.Strict);
        _cacheManager = new(MockBehavior.Strict);
        _cacheConfig = new RedisConfiguration()
        {
            TimeToExpireInSeconds = 60,
            TimeToRenewInSeconds = 20,
        };
    }

    [Theory]
    [AutoData]
    public async Task GetScheduleByAsync_GivenNoCachedAndExistentPersonId_ThenReturnVaccineSchedulesInformationsAndAddObjectInCache(Guid personId, CovidVaccineScheduleResponse expectedResponse)
    {
        // Arrange
        var expectedCacheKey = $"key-{personId}";
        _cacheManager
            .Setup(x => x.GetAsync<CovidVaccineScheduleResponse>(expectedCacheKey))
            .ReturnsAsync(null as CovidVaccineScheduleResponse);
        _schedulesRepository
            .Setup(x => x.GetByAsync(personId))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.AddAsync(expectedCacheKey, expectedResponse, TimeSpan.FromSeconds(_cacheConfig.TimeToExpireInSeconds)))
            .Returns(ValueTask.CompletedTask);
        var sut = GetService();

        // Act
        var result = await sut.GetScheduleByAsync(personId);

        // Assert
        result.Should().Be(expectedResponse);
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    [Theory]
    [AutoData]
    public async Task GetScheduleByAsync_GivenNoCachedAndNonexistentPersonId_ThenReturnNull(Guid personId)
    {
        // Arrange
        var expectedCacheKey = $"key-{personId}";
        _cacheManager
            .Setup(x => x.GetAsync<CovidVaccineScheduleResponse>(expectedCacheKey))
            .ReturnsAsync(null as CovidVaccineScheduleResponse);
        _schedulesRepository
            .Setup(x => x.GetByAsync(personId))
            .ReturnsAsync(null as CovidVaccineScheduleResponse);
        var sut = GetService();

        // Act
        var result = await sut.GetScheduleByAsync(personId);

        // Assert
        result.Should().BeNull();
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    [Theory]
    [AutoData]
    public async Task GetScheduleByAsync_GivenContainsCachedResponseWithPersonId_ThenReturnVaccineSchedulesInformationsInCache(Guid personId, CovidVaccineScheduleResponse expectedResponse)
    {
        // Arrange
        var expectedCacheKey = $"key-{personId}";
        _cacheManager
            .Setup(x => x.GetAsync<CovidVaccineScheduleResponse>(expectedCacheKey))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.KeyTimeToLiveAsync(expectedCacheKey))
            .ReturnsAsync(TimeSpan.MaxValue);
        _cacheManager
            .Setup(x => x.KeyTimeToLiveAsync(expectedCacheKey))
            .ReturnsAsync(TimeSpan.MaxValue);
        var sut = GetService();

        // Act
        var result = await sut.GetScheduleByAsync(personId);

        // Assert
        result.Should().Be(expectedResponse);
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    [Theory]
    [AutoData]
    public async Task GetScheduleByAsync_GivenCachedResponseWithPersonIdIsExpired_ThenGetVaccineSchedulesInformationsAndSetNewInCache(Guid personId, CovidVaccineScheduleResponse expectedResponse)
    {
        // Arrange
        var expectedCacheKey = $"key-{personId}";
        _cacheManager
            .Setup(x => x.GetAsync<CovidVaccineScheduleResponse>(expectedCacheKey))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.KeyTimeToLiveAsync(expectedCacheKey))
            .ReturnsAsync(TimeSpan.Zero);
        _schedulesRepository
            .Setup(x => x.GetByAsync(personId))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.AddAsync(expectedCacheKey, expectedResponse, TimeSpan.FromSeconds(_cacheConfig.TimeToExpireInSeconds)))
            .Returns(ValueTask.CompletedTask);
        var sut = GetService();

        // Act
        var result = await sut.GetScheduleByAsync(personId);

        // Assert
        result.Should().Be(expectedResponse);
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    [Theory]
    [AutoData]
    public async Task GetSchedulesAsync_GivenNoCached_ThenReturnListOfVaccineSchedulesInformationsAndSetObjectsInCache(DateTime startDate, IReadOnlyCollection<CovidVaccineScheduleResponse> expectedResponse)
    {
        // Arrange
        var requestParams = new
        {
            StartDate = DateOnly.FromDateTime(startDate),
            EndDate = DateOnly.FromDateTime(DateTime.Now),
        };
        var expectedCacheKey = $"key-{requestParams.StartDate:yyyyMMdd}-{requestParams.EndDate:yyyyMMdd}";
        _cacheManager
            .Setup(x => x.GetAsync<IReadOnlyCollection<CovidVaccineScheduleResponse>>(expectedCacheKey))
            .ReturnsAsync(null as IReadOnlyCollection<CovidVaccineScheduleResponse>);
        _schedulesRepository
            .Setup(x => x.GetAllAsync(requestParams.StartDate, requestParams.EndDate))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.AddAsync(expectedCacheKey, expectedResponse, TimeSpan.FromSeconds(_cacheConfig.TimeToExpireInSeconds)))
            .Returns(ValueTask.CompletedTask);
        var sut = GetService();

        // Act
        var result = await sut.GetSchedulesAsync(requestParams.StartDate, requestParams.EndDate);

        // Assert
        result.Should().ContainInOrder(expectedResponse);
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    [Theory]
    [AutoData]
    public async Task GetSchedulesAsync_GivenNoCachedAndNonexistentSchedulesInRangeOfData_ThenReturnNull(DateTime startDate, DateTime endDate)
    {
        // Arrange
        var requestParams = new
        {
            StartDate = DateOnly.FromDateTime(startDate),
            EndDate = DateOnly.FromDateTime(endDate),
        };
        var expectedCacheKey = $"key-{requestParams.StartDate:yyyyMMdd}-{requestParams.EndDate:yyyyMMdd}";
        _cacheManager
            .Setup(x => x.GetAsync<IReadOnlyCollection<CovidVaccineScheduleResponse>>(expectedCacheKey))
            .ReturnsAsync(null as IReadOnlyCollection<CovidVaccineScheduleResponse>);
        _schedulesRepository
            .Setup(x => x.GetAllAsync(requestParams.StartDate, requestParams.EndDate))
            .ReturnsAsync(null as IReadOnlyCollection<CovidVaccineScheduleResponse>);
        var sut = GetService();

        // Act
        var result = await sut.GetSchedulesAsync(requestParams.StartDate, requestParams.EndDate);

        // Assert
        result.Should().BeNull();
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    [Theory]
    [AutoData]
    public async Task GetSchedulesAsync_GivenContainsCachedResponse_ThenReturnListOfVaccineSchedulesInformationsInCache(DateTime startDate, IReadOnlyCollection<CovidVaccineScheduleResponse> expectedResponse)
    {
        // Arrange
        var requestParams = new
        {
            StartDate = DateOnly.FromDateTime(startDate),
            EndDate = DateOnly.FromDateTime(DateTime.Now),
        };
        var expectedCacheKey = $"key-{requestParams.StartDate:yyyyMMdd}-{requestParams.EndDate:yyyyMMdd}";
        _cacheManager
            .Setup(x => x.GetAsync<IReadOnlyCollection<CovidVaccineScheduleResponse>>(expectedCacheKey))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.KeyTimeToLiveAsync(expectedCacheKey))
            .ReturnsAsync(TimeSpan.MaxValue);
        var sut = GetService();

        // Act
        var result = await sut.GetSchedulesAsync(requestParams.StartDate, requestParams.EndDate);

        // Assert
        result.Should().ContainInOrder(expectedResponse);
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    [Theory]
    [AutoData]
    public async Task GetSchedulesAsync_GivenCachedResponseIsExpired_ThenGetListOfVaccineSchedulesInformationsAndSetNewInCache(DateTime startDate, IReadOnlyCollection<CovidVaccineScheduleResponse> expectedResponse)
    {
        // Arrange
        var requestParams = new
        {
            StartDate = DateOnly.FromDateTime(startDate),
            EndDate = DateOnly.FromDateTime(DateTime.Now),
        };
        var expectedCacheKey = $"key-{requestParams.StartDate:yyyyMMdd}-{requestParams.EndDate:yyyyMMdd}";
        _cacheManager
            .Setup(x => x.GetAsync<IReadOnlyCollection<CovidVaccineScheduleResponse>>(expectedCacheKey))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.KeyTimeToLiveAsync(expectedCacheKey))
            .ReturnsAsync(TimeSpan.Zero);
        _schedulesRepository
            .Setup(x => x.GetAllAsync(requestParams.StartDate, requestParams.EndDate))
            .ReturnsAsync(expectedResponse);
        _cacheManager
            .Setup(x => x.AddAsync(expectedCacheKey, expectedResponse, TimeSpan.FromSeconds(_cacheConfig.TimeToExpireInSeconds)))
            .Returns(ValueTask.CompletedTask);
        var sut = GetService();

        // Act
        var result = await sut.GetSchedulesAsync(requestParams.StartDate, requestParams.EndDate);

        // Assert
        result.Should().ContainInOrder(expectedResponse);
        Mock.VerifyAll(_cacheManager, _schedulesRepository);
    }

    private CovidVaccineSchedulesService GetService()
    {
        return new(_schedulesRepository.Object, _cacheManager.Object, _cacheConfig);
    }
}
