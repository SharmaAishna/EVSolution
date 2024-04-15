using System.Text.Json.Serialization;
using System.Text.Json;

namespace ChargingProfileGenerator.Domain.Helper
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the TimeSpan value as a string
            string timeSpanString = reader.GetString();

            // Parse the string to TimeSpan
            TimeSpan timeSpan;
            if (!TimeSpan.TryParse(timeSpanString, out timeSpan))
            {
                throw new JsonException("Invalid TimeSpan format.");
            }

            return timeSpan;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            // Write the TimeSpan value as a string
            writer.WriteStringValue(value.ToString());
        }
    }
}
