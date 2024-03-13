using PsbCGEnumerator.JsonConverters;

using System.Text.Json.Serialization;

namespace PsbCGEnumerator.Models
{
    public class LayerInfoModel
    {
        [JsonPropertyName("height")] public int Height { get; set; }
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("layers")] public List<LayerModel> Layers { get; set; } = new();
    }

    public class LayerModel
    {
        [JsonPropertyName("height")] public int Height { get; set; }
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("layer_id")] public int LayerId { get; set; }
        [JsonPropertyName("layer_type")] public int LayerType { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("opacity")] public int Opacity { get; set; }
        [JsonPropertyName("top")] public int Top { get; set; }
        [JsonPropertyName("left")] public int Left { get; set; }
        [JsonPropertyName("type")] public int Type { get; set; }
        [JsonPropertyName("visible")][JsonConverter(typeof(BooleanIntegerConverter))] public bool Visible { get; set; }
    }
}
