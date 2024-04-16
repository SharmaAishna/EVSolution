using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace ChargingProfileGenerator.Domain.Helper
{
    public class DecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(decimal));
            return decimal.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            // Round the value to 4 decimal places
            decimal roundedValue = Math.Round(value, 4);

            // Write the rounded value to the writer
            writer.WriteStringValue(roundedValue.ToString("0.0000"));
        }
    }
   
}
