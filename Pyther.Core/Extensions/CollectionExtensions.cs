using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Pyther.Core.Extensions
{
    /// <summary>
    /// Collections extensions methods
    /// </summary>
    public static class CollectionExtensions
    {
        #region IEnumerable<T>

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> enumerable, T value)
        {
            yield return value;

            foreach (var element in enumerable)
            {
                yield return element;
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, T value)
        {
            foreach (var element in enumerable)
            {
                yield return element;
            }
            yield return value;
        }

        public static TSource SafeMax<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
        {
            return source != null && source.Any() ? (source.Max() ?? defaultValue) : defaultValue;
        }

        public static TSource SafeMin<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
        {
            return source != null && source.Any() ? (source.Min() ?? defaultValue) : defaultValue;
        }

        public static int SafeMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int defaultValue)
        {
            return source != null && source.Any() ? source.Max(selector) : defaultValue;
        }

        public static int SafeMin<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int defaultValue)
        {
            return source != null && source.Any() ? source.Min(selector) : defaultValue;
        }

        public static long SafeMax<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long defaultValue)
        {
            return source != null && source.Any() ? source.Max(selector) : defaultValue;
        }

        public static long SafeMin<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long defaultValue)
        {
            return source != null && source.Any() ? source.Min(selector) : defaultValue;
        }

        public static float SafeMax<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float defaultValue)
        {
            return source != null && source.Any() ? source.Max(selector) : defaultValue;
        }

        public static float SafeMin<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float defaultValue)
        {
            return source != null && source.Any() ? source.Min(selector) : defaultValue;
        }

        public static double SafeMax<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue)
        {
            return source != null && source.Any() ? source.Max(selector) : defaultValue;
        }

        public static double SafeMin<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue)
        {
            return source != null && source.Any() ? source.Min(selector) : defaultValue;
        }

        public static decimal SafeMax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal defaultValue)
        {
            return source != null && source.Any() ? source.Max(selector) : defaultValue;
        }

        public static decimal SafeMin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal defaultValue)
        {
            return source != null && source.Any() ? source.Min(selector) : defaultValue;
        }

        public static TResult SafeMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
        {
            return source != null && source.Any() ? source.Max(selector) ?? defaultValue : defaultValue;
        }

        public static TResult SafeMin<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
        {
            return source != null && source.Any() ? source.Min(selector) ?? defaultValue : defaultValue;
        }

        #endregion

        #region ICollection<T>

        public static void Remove<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            foreach (var element in collection.Where(predicate).ToList())
            {
                collection.Remove(element);
            }
        }

        #endregion

        #region IList<T>

        public static void Remove<T>(this IList<T> list, Func<T, bool> predicate)
        {
            foreach (var element in list.Where(predicate).ToList())
            {
                list.Remove(element);
            }
        }

        public static bool AddUnique<T>(this IList<T> source, T element)
        {
            if (!source.Contains(element))
            {
                source.Add(element);
                return true;
            }
            return false;
        }

        #endregion

        #region List<T>

        /// <summary>
        /// Resize a List and fill it with default values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="newSize"></param>
        /// <param name="element"></param>
#pragma warning disable CS8601
        public static void Resize<T>(this List<T> list, int newSize, T element = default)
#pragma warning restore CS8601
        {
            int oldCount = list.Count;

            if (newSize < oldCount)
            {
                list.RemoveRange(newSize, oldCount - newSize);
            }
            else if (newSize > oldCount)
            {
                if (newSize > list.Capacity)
                {
                    list.Capacity = newSize;
                }
                list.AddRange(Enumerable.Repeat(element, newSize - oldCount));
            }
        }

        #endregion

        #region ObservableCollection<T>

        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparer)
        {
            var list = new List<T>(collection);
            list.Sort(comparer);

            for (int i = 0; i < list.Count; i++)
            {
                collection.Move(collection.IndexOf(list[i]), i);
            }
        }

        #endregion

        #region NameValueCollection

        /// <summary>
        /// Convert a NameValueCollection to IEnumerable of string KeyValuePairs.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<KeyValuePair<string, string>> ToPairs(this NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            return collection.Cast<string>().Select(key => new KeyValuePair<string, string>(key, collection[key]));
        }

        /// <summary>
        /// Convert a NameValueCollection to ILookup of strings.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ILookup<string, string> ToLookup(this NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            var pairs =
                from key in collection.Cast<String>()
                from value in collection.GetValues(key)
                select new { key, value };

            return pairs.ToLookup(pair => pair.key, pair => pair.value);
        }

        #endregion
    }
}
