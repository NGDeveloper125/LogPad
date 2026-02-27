using System.Text.Json.Serialization;

namespace LogPad.Entities;

public record FileDialogResult
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}
