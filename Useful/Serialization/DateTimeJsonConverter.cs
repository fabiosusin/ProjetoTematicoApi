using Newtonsoft.Json;
using System;

namespace Useful.Serialization
{
    public class DateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(DateTime) || objectType == typeof(DateTime?);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value.GetType() == typeof(DateTime))
                return reader.Value;

            string rawDate = (string)reader.Value;
            if (DateTime.TryParse(rawDate, out DateTime date)) return date;
            if (objectType == typeof(DateTime?)) { return null; }
            return DateTime.MinValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
