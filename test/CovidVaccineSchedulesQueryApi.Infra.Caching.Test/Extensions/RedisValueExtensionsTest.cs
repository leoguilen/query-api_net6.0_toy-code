namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Extensions;

[Trait("Unit", nameof(RedisValueExtensions))]
public class RedisValueExtensionsTest
{
    [Theory]
    [AutoData]
    public async Task As_GivenARedisValue_ThenReturnExpectedObject(Fixture fixture)
    {
        // Arrange
        var expectedObject = fixture.Create<TestObject>();
        var redisValue = (RedisValue)expectedObject.ToBytes();

        // Act
        var result = await redisValue
            .As<TestObject>();

        // Assert
        result.Should().BeEquivalentTo(expectedObject);
    }

    [Theory]
    [AutoData]
    public async Task As_GivenArrayOfRedisValue_ThenReturnExpectedObjects(Fixture fixture)
    {
        // Arrange
        var expectedObjects = fixture
            .CreateMany<TestObject>();
        var arrayOfRedisValue = new RedisValue[]
        {
            JsonSerializer.SerializeToUtf8Bytes(expectedObjects),
        };

        // Act
        var result = await arrayOfRedisValue
            .As<IEnumerable<TestObject>>();

        // Assert
        result.Should().BeEquivalentTo(expectedObjects);
    }

    [Fact]
    public async Task As_GivenRedisValueIsNull_ThenReturnDefaultObjectValue()
    {
        // Arrange
        var invalidRedisValue = RedisValue.Null;

        // Act
        var result = await invalidRedisValue
            .As<TestObject>();

        // Assert
        result.Should().Be(default(TestObject));
    }

    [Fact]
    public async Task As_GivenRedisValueIsEmpty_ThenReturnDefaultObjectValue()
    {
        // Arrange
        var invalidRedisValue = RedisValue.EmptyString;

        // Act
        var result = await invalidRedisValue
            .As<TestObject>();

        // Assert
        result.Should().Be(default(TestObject));
    }

    [Fact]
    public async Task As_GivenArrayOfRedisValueIsNull_ThenReturnDefaultObjectValue()
    {
        // Arrange
        var invalidRedisValue = null as RedisValue[];

        // Act
        var result = await invalidRedisValue
            .As<IEnumerable<TestObject>>();

        // Assert
        result.Should().Equals(default(TestObject));
    }

    [Fact]
    public async Task As_GivenArrayOfRedisValueIsEmpty_ThenReturnDefaultObjectValue()
    {
        // Arrange
        var invalidRedisValue = Array.Empty<RedisValue>();

        // Act
        var result = await invalidRedisValue
            .As<IEnumerable<TestObject>>();

        // Assert
        result.Should().Equals(default(TestObject));
    }
}
