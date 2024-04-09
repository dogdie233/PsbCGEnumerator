namespace PsbCGEnumerator.Models
{
    public class ResxModel
    {
        // public int PsbVersion { get; set; }
        public string PsbType { get; set; } = string.Empty;
        // public string Platform { get; set; } = "none";
        // public string? CryptKey { get; set; }
        // public bool ExternalTextures { get; set; } = false;
        // public object? Context { get; set; } = null;  // Context: {}
        public Dictionary<string, string> Resources { get; set; } = new();
    }
}
