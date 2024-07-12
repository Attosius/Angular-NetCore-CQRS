using System;
using System.Collections.Generic;
using System.Linq;

namespace PromomashInc.Helpers.Extensions
{
    public static class CommonExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool NotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static string WrapString(this string str, string wraper = "\"")
        {
            return $"{wraper}{str}{wraper}";
        }

        public static string TrimStart(this string str, int num)
        {
            if (str.IsNullOrEmpty() || str.Length < num + 1)
            {
                return str;
            }
            return str.Substring(0, num);
        }

        public static string WrapBrackets(this string str, string bracketLeft = "[", string bracketRight = "]")
        {
            return $"{bracketLeft}{str}{bracketRight}";
        }

        public static T GetValueOrDefault<TKey, T>(this Dictionary<TKey, T> dict, TKey key, T defaultValue)
        {
            T val;
            return dict.TryGetValue(key, out val) ? val : defaultValue;
        }
        public static T GetValueOrDefaultExt<TKey, T>(this IDictionary<TKey, T> dict, TKey key, T defaultValue)
        {
            T val;
            return dict.TryGetValue(key, out val) ? val : defaultValue;
        }

        public static Dictionary<string, object> CopyDictionary(this Dictionary<string, object> context)
        {
            var newContext = new Dictionary<string, object>();
            if (context == null) return newContext;
            foreach (var o in context)
            {
                newContext[o.Key] = o.Value;
            }

            return newContext;
        }

        public static string ToFirstTitleCase(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var first = str[0].ToString().ToUpper();
            return $"{first}{str.Substring(1)}";
        }

        public static double Round(this double d, int digints = 2)
        {
            return Math.Round(d, digints);
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            if (list.Count() < parts)
                return new List<IEnumerable<T>>
                {
                    list
                };
            int i = 0;
            var splits = list.GroupBy(item => i++ % parts).Select(part => part.AsEnumerable());
            return splits;
        }


        public static char LineBreak = '\n';
        public static string Quote = "\"";
    }
}
