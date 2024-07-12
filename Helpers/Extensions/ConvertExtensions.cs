using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Helpers.Extensions
{
    public static class ConvertExtensions
    {
        public static bool ToBool(this object o, bool defaultValue)
        {
            if (o == null)
                return defaultValue;
            bool res;
            return bool.TryParse(o.ToString(), out res) ? res : defaultValue;
        }

        public static int? ToNullableInt32(this string str)
        {
            if (int.TryParse(str, out var res))
                return res;
            return null;
        }

        public static long? ToNullableLong(this string str)
        {
            if (long.TryParse(str, out var res))
                return res;
            return null;
        }

        public static bool? ToNullableBool(this string str)
        {
            if (bool.TryParse(str, out var res))
                return res;
            return null;
        }

        public static long ToLong(this string str, long defaultVaue = 0)
        {
            if (long.TryParse(str, out var res))
                return res;
            return defaultVaue;
        }
        public static decimal ToDecimal(this string str, long defaultVaue = 0)
        {
            if (decimal.TryParse(str, out var res))
                return res;
            return defaultVaue;
        }

        public static string ToISOUtcDateString(this DateTime date)
        {
            //            String.Compare("", ""));
            // return date.ToUniversalTime().ToString("O");
            return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
        }
        public static string ToISODateStringForDb(this DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss");
        }
        public static DateTime ToTimeZoneFromTzToUtc(this DateTime date, string timeZoneId = "UTC")
        {
            if (timeZoneId.IsNullOrEmpty())
            {
                return date;
            }
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            var timeZoneDateTime = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);

            timeZoneDateTime = TimeZoneInfo.ConvertTimeToUtc(timeZoneDateTime, timeZoneInfo);
            return timeZoneDateTime;
        }
        public static DateTime ToTimeZoneFromUtcToTz(this DateTime date, string timeZoneId = "UTC")
        {
            if (timeZoneId.IsNullOrEmpty())
            {
                return date;
            }
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            var timeZoneDateTime = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);

            timeZoneDateTime = TimeZoneInfo.ConvertTimeFromUtc(timeZoneDateTime, timeZoneInfo);
            return timeZoneDateTime;
        }

        public static string ToHHmm(this TimeSpan date)
        {
            return date.ToString(@"hh\:mm");
        }

        public static string ToISODateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
        }

        public static DateTimeOffset ToEmptyIfNull(this DateTimeOffset? date)
        {
            return date ?? DateTimeOffset.MinValue;
        }
        public static string ToISODateStringForDb(this DateTimeOffset date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static DateTime ToDateTimeFromISOUtcDateString(this string str)
        {
            return DateTime.TryParse(str, out var res) ? res.ToUniversalTime() : DateTime.MinValue.ToUniversalTime();
        }

        public static DateTime ToDateTimeFromISODateString(this string str)
        {
            return DateTime.TryParse(str, out var res) ? res : DateTime.MinValue;
        }

        public static DateTime? ToDateTimeFromISODateStringNullable(this string str)
        {
            return DateTime.TryParse(str, out var res) ? res : (DateTime?)null;
        }

        public static string ToStringToday(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }



        public static int ToInt32(this string str, int defaultValue)
        {
            int res;
            return int.TryParse(str, out res) ? res : defaultValue;
        }

        public static string ToStringFromNullable(this object o)
        {
            if (o == null)
                return string.Empty;
            return o.ToString();
        }
        public static string ToStringFromNullable(this object o, string def)
        {
            if (o == null)
                return def;
            return o.ToString();
        }

        public static string ToEmptyIfNullString<T>(this T o)
        {
            return o == null ? string.Empty : o.ToString();
        }
        public static string ToEmptyIfNullStringTrim<T>(this T o)
        {
            return o == null ? string.Empty : o.ToString().Trim();
        }

        public static T ToEmptyIfNullObject<T>(this T o) where T : class, new()
        {
            return o ?? new T();
        }

        public static Guid ToGuid(this string str)
        {
            Guid guid;
            Guid.TryParse(str, out guid);
            return guid;
        }

        public static int ToInt(this string str, int defaultValue)
        {
            return int.TryParse(str, out int i) ? i : defaultValue;
        }
        public static bool ToBool(this string str, bool defaultValue)
        {
            return bool.TryParse(str, out bool i) ? i : defaultValue;
        }

        public static int ToIntFromObject(this object o, int defaultValue)
        {
            if (o == null)
                return defaultValue;
            return o.ToString().ToInt32(defaultValue);
        }
        public static T ToTFromObject<T>(this object o, T defaultValue)
        {
            if (o == null)
                return defaultValue;
            return (T)o;
        }


        public static List<T> ToListFromObject<T>(this T o)
        {
            return o == null ? new List<T>() : new List<T> { o };
        }

        public static ICollection<T> ToEmptyCollection<T>(this ICollection<T> o)
        {
            return o ?? new Collection<T>();
        }

        public static List<T> ToEmptyIfNullList<T>(this IList<T> o)
        {
            return o.IsNullOrCountZero() ? new List<T>() : o.ToList();
        }

        public static List<T> ToEmptyIfNullList<T>(this IEnumerable<T> o)
        {
            if (o == null)
                return new List<T>();

            var list = o as IList<T> ?? o.ToList();
            return list.IsNullOrCountZero() ? new List<T>() : list.ToList();
        }


        //        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        //        {
        //            return new HashSet<T>(source);
        //        }

        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return date.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.AddDays(1).AddMilliseconds(-1);
        }
        public static DateTime SetUnspecified(this DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
        }

        public static DateTime SetUtcKind(this DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public static DateTime? SetUtcKind(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return date;
            }
            return DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
        }

        public static DateTime TrimMilliseconds(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
        }

        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            var culture = CultureInfo.GetCultureInfo("en-GB");// System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-diff).Date;
        }



        public static Dictionary<TKey, TElement> ToDictionaryDistinctKey<TSource, TKey, TElement>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            source = source.DistinctByCommon(keySelector);
            return source.ToDictionary(keySelector, elementSelector);
        }

        public static T ToTypedObject<T>(this Dictionary<string, object> dictionary) where T : new()
        {
            var obj = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (dictionary.ContainsKey(prop.Name))
                {
                    prop.SetValue(obj, dictionary[prop.Name]);
                }
            }
            return obj;
        }

        public static List<T> ToTypedList<T>(this List<Dictionary<string, object>> listDict, HashSet<string> exclude) where T : new()
        {
            var list = new List<T>();
            exclude = exclude ?? new HashSet<string>();
            var propertyInfos = typeof(T).GetProperties();
            foreach (var dictionary in listDict)
            {
                var obj = new T();
                var caseDict = dictionary.ToDictionary(o => o.Key.ToUpper(), o => o.Value);
                foreach (var prop in propertyInfos)
                {
                    var upper = prop.Name.ToUpper();
                    if (exclude.Contains(upper))
                    {
                        continue;
                    }
                    if (caseDict.ContainsKey(upper))
                    {
                        prop.SetValue(obj, caseDict[upper]);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public static T ToTypedObjectCaseInSensitive<T>(this Dictionary<string, object> dictionary) where T : new()
        {
            var obj = new T();
            var caseDict = dictionary.ToDictionary(o => o.Key.ToUpper(), o => o.Value);
            foreach (var prop in typeof(T).GetProperties())
            {
                if (caseDict.ContainsKey(prop.Name.ToUpper()))
                {
                    prop.SetValue(obj, caseDict[prop.Name.ToUpper()]);
                }
            }
            return obj;
        }
        public static T ToTypedObjectCaseInSensitiveExtended<T>(this Dictionary<string, object> dictionary, T targeObj) where T : new()
        {
            var obj = new T();
            if (targeObj != null)
            {
                obj = targeObj;
            }
            var caseDict = dictionary.ToDictionary(o => o.Key.ToUpper(), o => o.Value);
            foreach (var key in caseDict.Keys)
            {
                if (caseDict[key].ToEmptyIfNullStringTrim() == "NULL")
                {
                    caseDict[key] = null;
                }
            }
            var cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            cultureInfo.Calendar.TwoDigitYearMax = 2099;
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!caseDict.TryGetValue(prop.Name.ToUpper(), out var val) || val == null)
                {
                    continue;
                }

                var value = val.ToStringFromNullable();
                if (new[] { typeof(string) }.Contains(prop.PropertyType))
                {
                    prop.SetValue(obj, value.ToStringFromNullable());
                    continue;
                }
                if (new[] { typeof(bool), typeof(bool?) }.Contains(prop.PropertyType))
                {
                    if (int.TryParse(value, out int i))
                    {
                        prop.SetValue(obj, i == 1 ? true : false);
                        continue;
                    }
                    prop.SetValue(obj, bool.Parse(value));
                    continue;
                }
                if (new[] { typeof(int), typeof(int?) }.Contains(prop.PropertyType))
                {
                    prop.SetValue(obj, int.Parse(value.ToStringFromNullable()));
                    continue;
                }
                if (new[] { typeof(long), typeof(long?) }.Contains(prop.PropertyType))
                {
                    prop.SetValue(obj, long.Parse(value.ToStringFromNullable()));
                    continue;
                }
                if (new[] { typeof(decimal), typeof(decimal?) }.Contains(prop.PropertyType))
                {
                    prop.SetValue(obj, decimal.Parse(value.ToStringFromNullable()));
                    continue;
                }
                if (new[] { typeof(Guid), typeof(Guid?) }.Contains(prop.PropertyType))
                {
                    prop.SetValue(obj, Guid.Parse(value.ToStringFromNullable()));
                    continue;
                }

                if (new[] { typeof(DateTime), typeof(DateTime?) }.Contains(prop.PropertyType))
                {
                    try
                    {
                        if (DateTime.TryParseExact(value.ToStringFromNullable(), "yyyyMMdd", cultureInfo, DateTimeStyles.None, out var dateValue))
                        {
                            prop.SetValue(obj, dateValue);
                            continue;
                        }
                        var dateTime = DateTime.Parse(value.ToStringFromNullable(), cultureInfo);
                        prop.SetValue(obj, dateTime);
                    }
                    catch (Exception)
                    {

                    }
                    continue;
                }
            }
            return obj;
        }

        public static T ToEnum<T>(this string enumStr, T defaultValue = default) where T : struct
        {
            return Enum.TryParse(enumStr, true, out T en) ? en : defaultValue;
        }

        public static HashSet<T> ToHashSet<T>(this List<T> data) // not IEnumerable (now net.standart2.0 is using, conflict with web proj)
        {
            return new HashSet<T>(data.ToEmptyIfNullList());
        }
    }
}
