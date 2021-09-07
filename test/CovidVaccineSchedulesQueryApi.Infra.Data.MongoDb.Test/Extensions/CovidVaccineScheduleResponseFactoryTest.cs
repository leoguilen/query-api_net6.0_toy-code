namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Test.Extensions;

using AutoFixture;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Documents;
using CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Factories;
using FluentAssertions;
using Xunit;

[Trait("Unit", nameof(CovidVaccineScheduleResponseFactory))]
public class CovidVaccineScheduleResponseFactoryTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void CreateFrom_GivenDocument_ThenCreateAndReturnResponseObject()
    {
        // Arrange
        var document = GetDocument();

        // Act
        var result = CovidVaccineScheduleResponseFactory
            .CreateFrom(document);

        // Assert
        result.PersonId.Should().Be(document.PersonId);
    }

    [Fact]
    public void CreateFrom_GivenDocumentIsNull_ThenReturnDefaultObjectValue()
    {
        // Arrange
        var document = null as ScheduleDocument;

        // Act
        var result = CovidVaccineScheduleResponseFactory
            .CreateFrom(document);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void CreateFrom_GivenListOfDocument_ThenCreateAndReturnListOfResponse()
    {
        // Arrange
        var listOfDocuments = new ScheduleDocument[] { GetDocument() };

        // Act
        var result = CovidVaccineScheduleResponseFactory
            .CreateFrom(listOfDocuments);

        // Assert
        result.Should().HaveCount(listOfDocuments.Length);
    }

    [Fact]
    public void CreateFrom_GivenListOfDocumentIsNull_ThenReturnEmptyListOfResponse()
    {
        // Arrange
        var listOfDocuments = null as ScheduleDocument[];

        // Act
        var result = CovidVaccineScheduleResponseFactory
            .CreateFrom(listOfDocuments);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void CreateFrom_GivenListOfDocumentIsEmpty_ThenReturnEmptyListOfResponse()
    {
        // Arrange
        var listOfDocuments = Array.Empty<ScheduleDocument>();

        // Act
        var result = CovidVaccineScheduleResponseFactory
            .CreateFrom(listOfDocuments);

        // Assert
        result.Should().BeEmpty();
    }

    private ScheduleDocument GetDocument() => new()
    {
        PersonId = Guid.NewGuid(),
        PersonName = _fixture.Create<string>(),
        PersonDocIdentifier = _fixture.Create<string>(),
        PersonVaccineSchedules = new PersonVaccineSchedulesDocument[]
        {
            new()
            {
                DoseNumber = _fixture.Create<byte>(),
                LocalDate = _fixture.Create<DateTime>(),
                LotIdentifier = _fixture.Create<string>(),
                ManufacturerName = _fixture.Create<string>(),
                ServiceUnit = _fixture.Create<string>(),
                VaccinatorName = _fixture.Create<string>(),
                WasApplied = _fixture.Create<bool>(),
            },
        },
    };
}
