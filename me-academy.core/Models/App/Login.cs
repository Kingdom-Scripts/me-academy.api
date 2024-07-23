namespace me_academy.core.Models.App;

public class Login : BaseAppModel
{
    public int UserId { get; set; }
    public string HashedToken { get; set; }
    public string Domain { get; set; }
    public DateTime ExpiresAt { get; set; }

    public User User { get; set; }
}
