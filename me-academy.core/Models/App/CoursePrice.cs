using me_academy.core.Interfaces;

namespace me_academy.core.Models.App;

public class CoursePrice : BaseAppModel, ISoftDeletable
{
    public int CourseId { get; set; }
    public int DurationId { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public int? DeletedById { get; set; }
    public DateTime? DeletedOn { get; set; }

    public virtual Course? Course { get; set; }
    public virtual Duration? Duration { get; set; }
}