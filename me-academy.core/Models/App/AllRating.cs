namespace me_academy.core.Models.App;
public class AllRating : BaseAppModel
{
    public int ContentId { get; set; }
    public int UserId { get; set; }
    public short Rating { get; set; }
    public string Comment { get; set; }
}
