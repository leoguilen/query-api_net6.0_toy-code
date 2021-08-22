namespace CovidVaccineSchedulesQueryApi.Core.Configurations
{
    public record VaultConfiguration
    {
        public bool UseVault { get; internal init; }

        public string ClientId { get; internal init; }

        public string ClientSecret { get; internal init; }

        public string Endpoint { get; internal init; }
    }
}
