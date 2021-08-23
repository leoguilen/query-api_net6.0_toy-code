namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Extensions;

using AutoFixture;
using AutoFixture.Xunit2;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;
using CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Models;
using FluentAssertions;
using StackExchange.Redis;
using Xunit;

[Trait("Unit", nameof(HashEntryExtensions))]
public class HashEntryExtensionsTest
{
    [Theory]
    [AutoData]
    public void ToHashEntries_GivenAObject_ThenReturnProprietiesInHashEntries(Fixture fixture)
    {
        // Arrange
        var obj = new TestObject()
        {
            Name = fixture.Create<string>(),
            Age = fixture.Create<sbyte>(),
        };

        // Act
        var result = obj.ToHashEntries();

        // Assert
        result.Should().SatisfyRespectively(
            res1 =>
            {
                res1.Name.Should().Be("Name");
                res1.Value.Should().Be(obj.Name);
            },
            res2 =>
            {
                res2.Name.Should().Be("Age");
                res2.Value.Should().Be(obj.Age);
            });
    }

    [Theory]
    [AutoData]
    public void ConvertTo_GivenAHashEntries_ThenReturnExpectedType(Fixture fixture)
    {
        // Arrange
        var hashEntries = new HashEntry[]
        {
            new HashEntry(nameof(TestObject.Name), fixture.Create<string>()),
            new HashEntry(nameof(TestObject.Age), fixture.Create<sbyte>()),
        };

        // Act
        var result = hashEntries.ConvertTo<TestObject>();

        // Assert
        result.Should().As<TestObject>();
    }
}
