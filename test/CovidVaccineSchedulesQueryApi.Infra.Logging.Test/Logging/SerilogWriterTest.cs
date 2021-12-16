namespace CovidVaccineSchedulesQueryApi.Infra.Logging.Test.Logging;

using AutoFixture;
using CovidVaccineSchedulesQueryApi.Infra.Logging.Logging;
using CovidVaccineSchedulesQueryApi.Infra.Logging.Models;
using Moq;
using Serilog;
using Xunit;

[Trait("Unit", nameof(SerilogWriter))]
public class SerilogWriterTest
{
    private const string LogMessageTemplate = "{@LogMessage}";

    private readonly Mock<ILogger> _logger;
    private readonly IFixture _fixture;

    public SerilogWriterTest()
    {
        _logger = new(MockBehavior.Strict);
        _fixture = new Fixture();
    }

    [Fact]
    public void Info_WithMessage_ThenShouldBeLoggedWithInfoLevel()
    {
        // Arrange
        _logger.Setup(x => x.Information(LogMessageTemplate, It.IsAny<LogMessage>()));
        var sut = GetSerilogWriter();

        // Act
        sut.Info(
            message: _fixture.Create<string>(),
            data: _fixture.Create<object>());

        // Assert
        _logger.VerifyAll();
    }

    [Fact]
    public void Warn_WithMessage_ThenShouldBeLoggedWithWarnLevel()
    {
        // Arrange
        _logger.Setup(x => x.Warning(LogMessageTemplate, It.IsAny<LogMessage>()));
        var sut = GetSerilogWriter();

        // Act
        sut.Warn(
            message: _fixture.Create<string>(),
            data: _fixture.Create<object>());

        // Assert
        _logger.VerifyAll();
    }

    [Fact]
    public void Error_WithMessage_ThenShouldBeLoggedWithErrorLevel()
    {
        // Arrange
        _logger.Setup(x => x.Error(LogMessageTemplate, It.IsAny<LogMessage>()));
        var sut = GetSerilogWriter();

        // Act
        sut.Error(
            message: _fixture.Create<string>(),
            data: _fixture.Create<object>());

        // Assert
        _logger.VerifyAll();
    }

    [Fact]
    public void Fatal_WithMessage_ThenShouldBeLoggedWithFatalLevel()
    {
        // Arrange
        _logger.Setup(x => x.Fatal(LogMessageTemplate, It.IsAny<LogMessage>()));
        var sut = GetSerilogWriter();

        // Act
        sut.Fatal(
            message: _fixture.Create<string>(),
            data: _fixture.Create<object>());

        // Assert
        _logger.VerifyAll();
    }

    private SerilogWriter GetSerilogWriter() => new(_logger.Object);
}
