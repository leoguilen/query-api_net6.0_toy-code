namespace CovidVaccineSchedulesQueryApi.Infra.Caching.Extensions;

using StackExchange.Redis;

internal static class HashEntryExtensions
{
    public static HashEntry[] ToHashEntries(this object obj)
    {
        var properties = obj.GetType().GetProperties();
        return properties
            .Select(property => new HashEntry(property.Name, property.GetValue(obj).ToString()))
            .ToArray();
    }

    public static T ConvertTo<T>(this HashEntry[] hashEntries)
    {
        var properties = typeof(T).GetProperties();
        var obj = Activator.CreateInstance(typeof(T));

        foreach (var property in properties)
        {
            var entry = Array
                .Find(hashEntries, g => g.Name.ToString()
                .Equals(property.Name, StringComparison.Ordinal));

            if (entry.Equals(default))
            {
                continue;
            }

            property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), property.PropertyType));
        }

        return (T)obj;
    }
}
