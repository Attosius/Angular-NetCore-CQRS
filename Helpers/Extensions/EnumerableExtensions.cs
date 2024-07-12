using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers.Extensions
{
    public static class EnumerableExtensions
    {
        public static string ToStringFromEnumerable(this IEnumerable<string> enumerable, string delimiter = ";")
        {
            var list = enumerable.ToList();
            if (list.IsNullOrCountZero())
            {
                return string.Empty;
            }
            return list.Aggregate((accumulate, item) => $"{accumulate}{delimiter}{item}");
        }

        public static string ToStringFromEnumerable<T>(this IEnumerable<T> enumerable, string delimiter = ";")
        {
            var list = enumerable.ToList();
            if (list.IsNullOrCountZero())
            {
                return string.Empty;
            }
            return list.Select(o => o.ToString()).Aggregate((accumulate, item) => $"{accumulate}{delimiter}{item}");
        }

        public static IEnumerable<T> AddItem<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Concat(item.ToListFromObject());
        }

        public static bool IsNullOrCountZero<T>(this IList<T> o)
        {
            return o == null || !o.Any();
        }
        public static bool NotNullOrCountZero<T>(this IList<T> o)
        {
            return o != null && o.Any();
        }

        public static bool IsNullOrCountZero<T>(this IEnumerable<T> o)
        {
            return o == null || !o.Any();
        }

        public static T FirstOrNew<T>(this IEnumerable<T> source) where T : class, new()
        {
            if (source == null)
            {
                return new T();
            }

            IList<T> list = source as IList<T>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            else
            {
                using (IEnumerator<T> e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        return e.Current;
                    }
                }
            }

            return new T();
        }

        public static TSource FirstOrNew<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            where TSource : new()
        {
            if (source == null)
                return new TSource();
            if (predicate == null)
                return new TSource();

            foreach (TSource element in source)
            {
                if (predicate(element)) return element;
            }
            return new TSource();
        }


        public static IEnumerable<T> DistinctByCommon<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.FirstOrDefault()).ToList();
        }
    }
}
