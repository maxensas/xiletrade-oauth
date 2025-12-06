using System.Text.Json.Serialization;

namespace XiletradeAuth.Models;

public sealed class PoeResponseToken
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }

    [JsonPropertyName("expires_in")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int Expires { get; set; }

    [JsonPropertyName("token_type")]
    public string Type { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("sub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Sub { get; set; }

    [JsonPropertyName("refresh_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Refresh { get; set; }
}
