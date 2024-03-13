using System.Text.Json;
using System.Text.Json.Serialization;

namespace PsbCGEnumerator.JsonConverters
{
    public class BooleanIntegerConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var integer = reader.GetInt32();
            return integer switch
            {
                0 => false,
                1 => true,
                _ => throw new JsonException("Invalid bool value")
            };
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value ? 1 : 0);
        }
    }
}
