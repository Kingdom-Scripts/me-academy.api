using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;
public class ContentLog : BaseAppModel
{
    public int ContentId { get; set; }
    [Required]
    public string ContentType { get; set; }
    [MaxLength(1000)]
    public required string Description { get; set; }
    public int CreatedById { get; set; }

    public ContentBase Content { get; set; }
    public User CreatedBy { get; set; }
}
