namespace CovidVaccineSchedulesQueryApi.Infra.Data.MongoDb.Documents;

internal record ScheduleDocument
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; internal init; }

    [BsonElement("PERSON_UUID")]
    [BsonRepresentation(BsonType.String)]
    public Guid PersonId { get; internal init; }

    [BsonElement("PERSON_NAME")]
    public string PersonName { get; internal init; }

    [BsonElement("PERSON_DOCIDENTIFIER")]
    public string PersonDocIdentifier { get; internal init; }

    [BsonElement("PERSON_VACCINE_SCHEDULES")]
    public IReadOnlyList<PersonVaccineSchedulesDocument> PersonVaccineSchedules { get; internal init; }
}
