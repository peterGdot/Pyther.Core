using System.Collections.ObjectModel;

namespace Pyther.Core.Extensions
{
    /// <summary>
    /// Collections extensions methods
    /// </summary>
    public static class CollectionExtensions
    {
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

        /// <summary>
        /// Remove elements from an "ObservableCollection" using a predicate callback.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int Remove<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            var listToRemove = collection.Where(predicate).ToList();

            foreach (var element in listToRemove)
            {
                collection.Remove(element);
            }

            return listToRemove.Count;
        }

        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparer)
        {
            var list = new List<T>(collection);
            list.Sort(comparer);

            for (int i = 0; i < list.Count; i++)
            {
                collection.Move(collection.IndexOf(list[i]), i);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null) return;
            foreach (var e in enumerable)
            {
                action(e);
            }
        }

        public static T? FindFirst<T>(this System.Collections.IList source, Func<T, bool> predicate)
        {
            foreach (T item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return default;
        }

        public static T? FindFirst<T>(this System.Collections.Generic.IList<T> source, Func<T, bool> predicate)
        {
            foreach (T item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return default;
        }

        public static bool AddUnique<T>(this System.Collections.Generic.IList<T> source, T element)
        {
            if (!source.Contains(element))
            {
                source.Add(element);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
