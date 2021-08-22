namespace CovidVaccineSchedulesQueryApi.Core.Configurations
{
    public record MongoDbConfiguration
    {
        public string ConnectionString { get; internal init; }
    }
}
