using Newtonsoft.Json;

namespace Desktop.Samples.Common
{
    public static class Extensions
    {
        public static string ToJson<T>(this T obj, bool isFormatting = false) =>
            JsonConvert.SerializeObject(obj, isFormatting ? Formatting.Indented : Formatting.None);

        public static T ParseTo<T>(this string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
