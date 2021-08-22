namespace CovidVaccineSchedulesQueryApi.Core.Configurations
{
    public record RedisConfiguration
    {
        public string ConnectionString { get; internal init; }

        public int TimeToExpireInSeconds { get; internal init; }

        public int TimeToRenewInSeconds { get; internal init; }
    }
}
