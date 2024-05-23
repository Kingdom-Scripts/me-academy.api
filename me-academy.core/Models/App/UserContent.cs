namespace me_academy.core.Models.App;

public class UserContent : BaseAppModel
{
    public int UserId { get; set; }

    public int OrderId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
