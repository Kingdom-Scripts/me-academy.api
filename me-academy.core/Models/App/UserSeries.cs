namespace me_academy.core.Models.App;

public class UserSeries : BaseAppModel
{
    public int UserId { get; set; }
    public int SeriesId { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime ExpiresOnUtc { get; set; }

    public Series Series { get; set; }
}
