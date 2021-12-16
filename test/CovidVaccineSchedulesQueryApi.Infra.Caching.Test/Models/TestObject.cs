namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Test.Models;

public record TestObject
{
    public string Name { get; init; }

    public sbyte Age { get; init; }

    public byte[] ToBytes() => JsonSerializer.SerializeToUtf8Bytes(this);
}
