namespace CovidVaccineSchedulesQueryApi.IntegrationTest.Setup;

public abstract class ControllerTestsBase :
    IClassFixture<WebApplicationFactoryTest>,
    IAsyncLifetime
{
    protected const string DefaultControllerRoute = "api/v1/schedules";

    protected ControllerTestsBase(WebApplicationFactoryTest factoryTest)
    {
        var mongoDatabase = factoryTest
            .Services
            .GetRequiredService<IMongoDatabase>();

        MongoDbFixture = new MongoDbFixture(mongoDatabase);
        Client = factoryTest.CreateClient();
    }

    protected MongoDbFixture MongoDbFixture { get; }

    protected HttpClient Client { get; }

    public Task DisposeAsync()
    {
        MongoDbFixture.Dispose();
        Client.Dispose();
        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        await MongoDbFixture
            .CreateCollectionAndAddTestDocumentsAsync();
    }
}
