using System.ComponentModel.DataAnnotations.Schema;

namespace me_academy.core.Models.App;

public class UserContents : BaseAppModel
{
    public int UserId { get; set; }

    public int OrderId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public User User { get; set; }
    [ForeignKey("OrderId")]
    public Order Order { get; set; }
}
