namespace CovidVaccineSchedulesQueryApi.IntegrationTest.Setup;

public class WebApplicationFactoryTest :
    WebApplicationFactory<Startup>,
    IAsyncLifetime
{
    public WebApplicationFactoryTest() =>
        ContainersFixture = new();

    public ContainersFixture ContainersFixture { get; }

    public HttpClient Client { get; private set; }

    public async Task InitializeAsync()
    {
        await ContainersFixture.InitializeAsync();
        Client = CreateClient();
    }

    public async Task DisposeAsync() =>
        await ContainersFixture.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Testing")
            .ConfigureTestServices(services => services
                .AddSingleton(_ => new MongoDbConfiguration()
                {
                    ConnectionString = ContainersFixture.MongoContainer.ConnectionString,
                })
                .AddSingleton(_ => new RedisConfiguration()
                {
                    ConnectionString = ContainersFixture.RedisContainer.ConnectionString,
                    TimeToExpireInSeconds = int.MaxValue,
                    TimeToRenewInSeconds = int.MaxValue,
                }));
    }
}
