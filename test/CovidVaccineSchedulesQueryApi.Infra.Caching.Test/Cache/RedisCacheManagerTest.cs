namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Cache;

using AutoFixture.Xunit2;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Cache;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Models;
using FluentAssertions;
using Moq;
using StackExchange.Redis;
using Xunit;

[Trait("Unit", nameof(RedisCacheManager))]
public class RedisCacheManagerTest
{
    private readonly Mock<IDatabaseAsync> _database;

    public RedisCacheManagerTest() =>
        _database = new(MockBehavior.Strict);

    [Theory]
    [AutoData]
    public async Task AddAsync_GivenCacheKeyAndAnyObject_ThenObjectAddedInCache(string cacheKey, TestObject obj, TimeSpan expireTime)
    {
        // Arrange
        _database
            .Setup(x => x.HashSetAsync(cacheKey, It.IsAny<HashEntry[]>(), CommandFlags.FireAndForget))
            .Returns(Task.CompletedTask);
        _database
            .Setup(x => x.KeyExpireAsync(cacheKey, expireTime, CommandFlags.FireAndForget))
            .ReturnsAsync(true);
        var sut = new RedisCacheManager(_database.Object);

        // Act
        await sut.AddAsync(cacheKey, obj, expireTime);

        // Assert
        _database.VerifyAll();
    }

    [Theory]
    [AutoData]
    public async Task DeleteAsync_GivenExistsKey_ThenKeyShouldBeRemoved(string cacheKey)
    {
        // Arrange
        _database
            .Setup(x => x.KeyDeleteAsync(cacheKey, CommandFlags.FireAndForget))
            .ReturnsAsync(true);
        var sut = new RedisCacheManager(_database.Object);

        // Act
        var result = await sut.DeleteAsync(cacheKey);

        // Assert
        result.Should().BeTrue();
        _database.VerifyAll();
    }

    [Theory]
    [AutoData]
    public async Task GetAsync_GivenExistentCachedDataByKey_ThenReturnObject(string cacheKey, TestObject expectedObj)
    {
        // Arrange
        _database
            .Setup(x => x.HashGetAllAsync(cacheKey, CommandFlags.None))
            .ReturnsAsync(expectedObj.ToHashEntries());
        var sut = new RedisCacheManager(_database.Object);

        // Act
        var result = await sut.GetAsync<TestObject>(cacheKey);

        // Assert
        result.Should().Be(expectedObj);
        _database.VerifyAll();
    }

    [Theory]
    [AutoData]
    public async Task GetAsync_GivenNonexistentCachedDataByKey_ThenReturnNullObject(string cacheKey)
    {
        // Arrange
        _database
            .Setup(x => x.HashGetAllAsync(cacheKey, CommandFlags.None))
            .ReturnsAsync(null as HashEntry[]);
        var sut = new RedisCacheManager(_database.Object);

        // Act
        var result = await sut.GetAsync<TestObject>(cacheKey);

        // Assert
        result.Should().BeNull();
        _database.VerifyAll();
    }

    [Theory]
    [AutoData]
    public async Task KeyTimeToLiveAsync_GivenExistsKey_ThenReturnCurrentExpirationTime(string cacheKey, TimeSpan expireTime)
    {
        // Arrange
        _database
            .Setup(x => x.KeyTimeToLiveAsync(cacheKey, CommandFlags.None))
            .ReturnsAsync(expireTime);
        var sut = new RedisCacheManager(_database.Object);

        // Act
        var result = await sut.KeyTimeToLiveAsync(cacheKey);

        // Assert
        result.Should().Be(expireTime);
        _database.VerifyAll();
    }
}
