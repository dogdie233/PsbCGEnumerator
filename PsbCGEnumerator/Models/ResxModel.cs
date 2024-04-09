using FreeMote;

using System.Text.Json.Serialization;

namespace PsbCGEnumerator.Models
{
    public class ResxModel
    {
        // public int PsbVersion { get; set; }
        // [JsonConverter(typeof(JsonStringEnumConverter))] public string PsbType { get; set; }
        // public string Platform { get; set; } = "none";
        // public string? CryptKey { get; set; }
        // public bool ExternalTextures { get; set; } = false;
        // public object? Context { get; set; } = null;  // Context: {}
        public Dictionary<string, string> Resources { get; set; } = new();
    }
}
