namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Cache;

using System.Text.Json;
using AutoFixture.Xunit2;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Cache;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Models;
using FluentAssertions;
using Moq;
using StackExchange.Redis;
using Xunit;

[Trait("Unit", nameof(RedisCacheManager))]
public class RedisCacheManagerTest
{
    private readonly Mock<IDatabase> _database;

    public RedisCacheManagerTest() =>
        _database = new(MockBehavior.Strict);

    [Theory]
    [AutoData]
    public async Task AddAsync_GivenCacheKeyAndAnyObject_ThenObjectAddedInCache(string cacheKey, TestObject obj, TimeSpan expireTime)
    {
        // Arrange
        _database
            .Setup(x => x.SetAddAsync(cacheKey, obj.ToBytes(), CommandFlags.FireAndForget))
            .ReturnsAsync(true);
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
            .Setup(x => x.SetMembersAsync(cacheKey, CommandFlags.None))
            .ReturnsAsync(new RedisValue[] { expectedObj.ToBytes() });
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
            .Setup(x => x.SetMembersAsync(cacheKey, CommandFlags.None))
            .ReturnsAsync(new RedisValue[] { RedisValue.Null });
        var sut = new RedisCacheManager(_database.Object);

        // Act
        var result = await sut.GetAsync<TestObject>(cacheKey);

        // Assert
        result.Should().BeNull();
        _database.VerifyAll();
    }

    [Theory]
    [AutoData]
    public async Task GetListAsync_GivenExistentCachedListByKey_ThenReturnList(string cacheKey, IEnumerable<TestObject> expectedList)
    {
        // Arrange
        var listOfRedisValue = new RedisValue[]
        {
            JsonSerializer.SerializeToUtf8Bytes(expectedList),
        };
        _database
            .Setup(x => x.SetMembersAsync(cacheKey, CommandFlags.None))
            .ReturnsAsync(listOfRedisValue);
        var sut = new RedisCacheManager(_database.Object);

        // Act
        var result = await sut
            .GetListAsync<IEnumerable<TestObject>>(cacheKey);

        // Assert
        result.Should().BeEquivalentTo(expectedList);
        _database.VerifyAll();
    }

    [Theory]
    [AutoData]
    public async Task GetListAsync_GivenNonexistentCachedListByKey_ThenReturnNull(string cacheKey)
    {
        // Arrange
        _database
            .Setup(x => x.SetMembersAsync(cacheKey, CommandFlags.None))
            .ReturnsAsync(Array.Empty<RedisValue>());
        var sut = new RedisCacheManager(_database.Object);

        // Act
        var result = await sut
            .GetListAsync<IEnumerable<TestObject>>(cacheKey);

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
