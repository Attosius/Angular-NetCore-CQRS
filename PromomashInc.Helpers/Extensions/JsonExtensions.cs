using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PromomashInc.Helpers.Extensions
{
    public static class JsonExtensions
    {

        public static string SafeSerialize(this object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            try
            {
                return JsonConvert.SerializeObject(obj, settings);
            }
            catch (Exception e)
            {
                return $"Error on serialize: {e}";
            }
        }

        public static string ToJsonString<T>(this T obj, TypeNameHandling typeHandle = TypeNameHandling.None)
        {
            return ObjectToJson(obj, typeHandle);
        }

        public static T JsonByteArrayToObject<T>(byte[] arrBytes) where T : new()
        {
            var str = Encoding.UTF8.GetString(arrBytes);

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
            return JsonConvert.DeserializeObject<T>(str, settings);
        }

        public static string ObjectToJson<T>(T obj, TypeNameHandling typeHandle = TypeNameHandling.None)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = typeHandle,
                Formatting = Formatting.Indented

            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T JsonToObject<T>(this string str, TypeNameHandling typeHandle = TypeNameHandling.None)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = typeHandle,
                Formatting = Formatting.Indented

            };

            return JsonConvert.DeserializeObject<T>(str, settings);
        }
        public static string ObjectToJsonSafe<T>(T obj)
        {
            try
            {
                return ObjectToJson(obj);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static DateTime? GetDateTimeByKey(this JObject jObject, string key)
        {
            if (jObject.ContainsKey(key) && jObject[key].Type == JTokenType.Date)
            {
                return jObject[key].Value<DateTime>();
            }

            return null;
        }

        public static DateTime GetDateTimeByKey(this JObject jObject, string key, DateTime defaultValue)
        {
            var val = jObject.GetDateTimeByKey(key);
            return val ?? defaultValue;
        }

        public static string GetStringByKey(this JObject jObject, string key, string defaultValue = null)
        {
            if (jObject.ContainsKey(key) && jObject[key].Type == JTokenType.String)
            {
                return jObject[key].ToString();
            }

            return defaultValue;
        }
        public static int GetIntByKey(this JObject jObject, string key, int defaultValue = default)
        {
            if (jObject.ContainsKey(key) && jObject[key].Type == JTokenType.Integer)
            {
                return jObject[key].Value<int>();
            }

            return defaultValue;
        }
        public static bool GetBoolByKey(this JObject jObject, string key, bool defaultValue = default)
        {
            if (jObject.ContainsKey(key) && jObject[key].Type == JTokenType.Boolean)
            {
                return jObject[key].Value<bool>();
            }

            return defaultValue;
        }

        public static JObject ToJObjectSafe(this string str)
        {
            try
            {

                var jsobj = JObject.Parse(str);
                return jsobj;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static T CopyBySerializeDeserialize<T>(this T obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
            try
            {
                if (obj == null)
                {
                    return default;
                }
                var ser = JsonConvert.SerializeObject(obj, settings);
                return JsonConvert.DeserializeObject<T>(ser);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static void CloneValues<T>(this T destination, T source)
        {
            foreach (var prop in destination.GetType().GetProperties())
            {
                var valueSource = prop.GetValue(source, null);
                if (valueSource == null)
                {
                    continue;
                }

                prop.SetValue(destination, valueSource, null);
            }
        }

        public static T CloneValues<T>(this T source)
        {
            var dest = Activator.CreateInstance<T>();
            dest.CloneValues(source);
            return dest;
        }

        public static List<T> CloneList<T>(this List<T> source)
        {
            return source.Select(t => t.CloneValues()).ToList();
        }
    }

}