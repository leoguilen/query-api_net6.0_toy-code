namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;

internal static class RedisValueExtensions
{
    public static async ValueTask<T> As<T>(this RedisValue value)
    {
        if (value.IsNullOrEmpty)
        {
            return default;
        }

        using var memoryStream = new MemoryStream(value);

        return await JsonSerializer
            .DeserializeAsync<T>(memoryStream);
    }

    public static async ValueTask<T> As<T>(this RedisValue[] values)
    {
        if (values?.Any() != true)
        {
            return default;
        }

        using var memoryStream = new MemoryStream();
        for (var index = 0; index < values?.Length; index++)
        {
            await memoryStream.WriteAsync(
                buffer: values[index],
                offset: 0,
                count: (int)values[index].Length());
        }

        memoryStream.Position = 0;

        return await JsonSerializer
            .DeserializeAsync<T>(memoryStream);
    }
}
