namespace CovidVaccineSchedulesQueryApi.Core.Extensions;

internal static class TypeExtensions
{
    public static bool IsIEnumerableOf(this Type type) =>
        type.GetInterfaces().Any(x => x.IsGenericType
        && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
}
