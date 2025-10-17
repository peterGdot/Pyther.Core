using System.Dynamic;

namespace Pyther.Core.Extensions;

public static class ExpandoObjectExtensions
{
    public static bool HasProperty(this ExpandoObject obj, string key)
    {
        return (obj as IDictionary<string, object>)?.ContainsKey(key) ?? false;
    }

    public static object? GetProperty(this ExpandoObject obj, string key)
    {
        return obj is IDictionary<string, object> dict && dict.TryGetValue(key, out object? result) ? result : null;
    }

    public static T? GetProperty<T>(this ExpandoObject obj, string key) where T : struct
    {
        if (obj == null) return null;
        string? data = obj is IDictionary<string, object> dict && dict.TryGetValue(key, out object? result) ? result.ToString() : null;
        return data?.Parse<T>();
    }

    public static int CountProperty(this ExpandoObject obj)
    {
        return obj != null ? ((IDictionary<string, object>)obj!).Count : 0;
    }

}
