namespace Pyther.Core.Extensions;

public static class ObjectExtensions
{
    public static int CountNonNullProperties(this object obj)
    {
        return obj?.GetType().GetProperties().Select(prop => prop.GetValue(obj, null)).Count(val => val != null) ?? 0;
    }
}
