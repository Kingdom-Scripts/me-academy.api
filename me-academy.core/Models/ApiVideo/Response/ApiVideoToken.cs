namespace me_academy.core.Models.ApiVideo.Response
{
    public class ApiVideoToken
    {
        public string Token { get; set; }
        public int Ttl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
