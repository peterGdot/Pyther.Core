using System.Text;

namespace Pyther.Core.Extensions;
public static class EnumerableExtensions
{
    /// <summary>
    /// Execute an action on each element of an `IEnumerable`.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        if (enumerable == null) return;
        foreach (var e in enumerable)
        {
            action(e);
        }
    }

    /// <summary>
    /// Join all elements of an `IEnumerable` to a string list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="separator"></param>
    /// <param name="start">optional string prefix to apply, if the result is not empty.</param>
    /// <returns></returns>
    public static string Join<T>(this IEnumerable<T> enumerable, string separator = ",", string? start = null)
    {
        return enumerable.Aggregate(new StringBuilder(start),
            (current, next) => current.Append(current.Length == 0 ? "" : separator).Append(next)
        ).ToString();
    }

    /// <summary>
    /// Ensure an empty IEnumerable if the list is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static IEnumerable<T> Ensure<T>(this IEnumerable<T>? enumerable)
    {
        if (enumerable == null)
        {
            yield break;
        }
        foreach (var item in enumerable)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Check if an IEnumerable is null or empty (zero elements).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static bool IsNullorEmpty<T>(this IEnumerable<T>? enumerable) => enumerable == null || !enumerable.Any();
}
