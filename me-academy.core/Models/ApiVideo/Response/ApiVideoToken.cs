using System.Text.Json.Serialization;

namespace me_academy.core.Models.ApiVideo.Response
{
    public class ApiVideoToken
    {
        public string Token { get; set; }
        [JsonIgnore]
        public int Ttl { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? ExpiresAt { get; set; }
    }
}
