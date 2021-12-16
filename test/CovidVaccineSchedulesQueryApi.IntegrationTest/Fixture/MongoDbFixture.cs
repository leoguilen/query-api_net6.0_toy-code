namespace CovidVaccineSchedulesQueryApi.IntegrationTest.Fixture;

public class MongoDbFixture : IDisposable
{
    private const string FirstPersonId = "2f668b10-127e-11ec-82a8-0242ac130003";
    private const string SecondPersonId = "34311944-127e-11ec-82a8-0242ac130003";
    private const string CollectionName = "PERSON_VACCINE_SCHEDULES";

    private readonly IMongoDatabase _mongoDatabase;

    public MongoDbFixture(IMongoDatabase mongoDatabase) =>
        _mongoDatabase = mongoDatabase;

    public async Task CreateCollectionAndAddTestDocumentsAsync()
    {
        await CreateCollectionAsync();
        await AddTestDocumentsAsync();
    }

    public void Dispose()
    {
        _mongoDatabase
            .DropCollection(CollectionName);
        GC.SuppressFinalize(this);
    }

    private static async Task CreateIndexAsync(IMongoCollection<BsonDocument> collection)
    {
        var indexBuilder = Builders<BsonDocument>
            .IndexKeys
            .Ascending("PERSON_UUID");
        var indexModel = new CreateIndexModel<BsonDocument>(indexBuilder);

        _ = await collection.Indexes.CreateOneAsync(indexModel);
    }

    private async Task CreateCollectionAsync() =>
        await _mongoDatabase
            .CreateCollectionAsync(CollectionName);

    private async Task AddTestDocumentsAsync()
    {
        var collection = _mongoDatabase
            .GetCollection<BsonDocument>(CollectionName);

        await CreateIndexAsync(collection);
        await collection.InsertManyAsync(new[]
        {
            new BsonDocument
            {
                { "PERSON_UUID", Guid.Parse(FirstPersonId).ToString() },
                { "PERSON_NAME", "Maria Antonela" },
                { "PERSON_DOCIDENTIFIER", "000.111.222-33" },
                {
                    "PERSON_VACCINE_SCHEDULES",
                    new BsonArray
                    {
                        new BsonDocument
                        {
                            { "DOSE_NUMBER", 1 },
                            { "SERVICE_UNIT", "USP Centro" },
                            { "LOCAL_DATE", new DateTime(2021, 3, 27, 8, 30, 15) },
                            { "LOT_IDENTIFIER", "123456" },
                            { "MANUFACTURER_NAME", "PFIZER" },
                            { "VACCINATOR_NAME", "João Marcos Da Silva" },
                            { "APPLIED_ST", true },
                        },
                        new BsonDocument
                        {
                            { "DOSE_NUMBER", 2 },
                            { "SERVICE_UNIT", "USP Centro" },
                            { "LOCAL_DATE", new DateTime(2021, 6, 26, 16, 44, 22) },
                            { "LOT_IDENTIFIER", "654321" },
                            { "MANUFACTURER_NAME", "PFIZER" },
                            { "VACCINATOR_NAME", "Larissa Sanches" },
                            { "APPLIED_ST", true },
                        },
                    }
                },
            },
            new BsonDocument
            {
                { "PERSON_UUID", Guid.Parse(SecondPersonId).ToString() },
                { "PERSON_NAME", "Marcos Do Amaral" },
                { "PERSON_DOCIDENTIFIER", "999.888.777-66" },
                {
                    "PERSON_VACCINE_SCHEDULES",
                    new BsonArray
                    {
                        new BsonDocument
                        {
                            { "DOSE_NUMBER", 1 },
                            { "SERVICE_UNIT", "USP Canelinha" },
                            { "LOCAL_DATE", new DateTime(2021, 4, 8, 13, 10, 26) },
                            { "LOT_IDENTIFIER", "101456" },
                            { "MANUFACTURER_NAME", "CORONAVAC" },
                            { "VACCINATOR_NAME", "Paulinia Da Silva" },
                            { "APPLIED_ST", true },
                        },
                        new BsonDocument
                        {
                            { "DOSE_NUMBER", 2 },
                            { "SERVICE_UNIT", "-" },
                            { "LOCAL_DATE", new DateTime(2021, 5, 10, 10, 30, 00) },
                            { "LOT_IDENTIFIER", "467821" },
                            { "MANUFACTURER_NAME", "CORONAVAC" },
                            { "VACCINATOR_NAME", "Paulo Cesar" },
                            { "APPLIED_ST", false },
                        },
                    }
                },
            },
        });
    }
}
