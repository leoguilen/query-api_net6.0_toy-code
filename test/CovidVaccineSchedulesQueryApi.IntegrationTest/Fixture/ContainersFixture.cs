namespace CovidVaccineSchedulesQueryApi.IntegrationTest.Fixture;

public class ContainersFixture : IAsyncLifetime
{
    public ContainersFixture()
    {
        MongoContainer = new DatabaseContainerBuilder<MongoDbContainer>()
            .Begin()
            .WithImage("mongo")
            .Build();

        RedisContainer = new DatabaseContainerBuilder<RedisContainer>()
            .Begin()
            .WithImage("redis")
            .Build();
    }

    public RedisContainer RedisContainer { get; }

    public MongoDbContainer MongoContainer { get; }

    public Task DisposeAsync() => Task.WhenAll(
            RedisContainer.Stop(),
            MongoContainer.Stop());

    public Task InitializeAsync() => Task.WhenAll(
            RedisContainer.Start(),
            MongoContainer.Start());
}
