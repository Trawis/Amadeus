using System.Text.Json.Serialization;

namespace Core.AmadeusModule.Messaging
{
  public class AuthResponse
  {
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("application_name")]
    public string Application_Name { get; set; }

    [JsonPropertyName("client_id")]
    public string Client_Id { get; set; }

    [JsonPropertyName("token_type")]
    public string Token_Type { get; set; }

    [JsonPropertyName("access_token")]
    public string Access_Token { get; set; }

    [JsonPropertyName("expires_in")]
    public string Expires_In { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }
  }
}
