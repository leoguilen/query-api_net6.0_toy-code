namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Documents;

using MongoDB.Bson.Serialization.Attributes;

internal record PersonVaccineSchedulesDocument
{
    [BsonElement("DOSE_NUMBER")]
    public byte DoseNumber { get; internal init; }

    [BsonElement("SERVICE_UNIT")]
    public string ServiceUnit { get; internal init; }

    [BsonElement("LOCAL_DATE")]
    public DateTimeOffset LocalDate { get; internal init; }

    [BsonElement("LOT_IDENTIFIER")]
    public string LotIdentifier { get; internal init; }

    [BsonElement("MANUFACTURER_NAME")]
    public string ManufacturerName { get; internal init; }

    [BsonElement("VACCINATOR_NAME")]
    public string VaccinatorName { get; internal init; }

    [BsonElement("APPLIED_ST")]
    public bool WasApplied { get; internal init; }
}
