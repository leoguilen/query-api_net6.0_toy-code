namespace CovidVaccineSchedulesQueryApi.IntegrationTest.Containers;

public class MongoDbContainer : DatabaseContainer
{
    private const int DefaultPort = 27017;
    private const string DefaultDatabaseName = "VACCINE_SCHEDULES";

    private IMongoClient _mongoClient;

    public override string DatabaseName => base.DatabaseName ?? DefaultDatabaseName;

    public override string ConnectionString => $"mongodb://{GetDockerHostIpAddress()}:{GetMappedPort(DefaultPort)}";

    public IMongoDatabase MongoDatabase { get; private set; }

    private IMongoClient MongoClient => _mongoClient ??= new MongoClient(ConnectionString);

    protected override async Task WaitUntilContainerStarted()
    {
        await base.WaitUntilContainerStarted();

        var result = await Policy
            .TimeoutAsync(TimeSpan.FromMinutes(2))
            .WrapAsync(Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(10)))
            .ExecuteAndCaptureAsync(() =>
            {
                MongoDatabase = MongoClient.GetDatabase(DatabaseName);
                return Task.CompletedTask;
            });

        if (result.Outcome == OutcomeType.Failure)
        {
            MongoDatabase = null;
            throw result.FinalException;
        }
    }
}
