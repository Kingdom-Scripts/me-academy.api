namespace me_academy.core.Models.Input.Courses;

public class CourseSearchModel : PagingOptionModel
{
    public string? SearchQuery { get; set; }
    public bool? IsActive { get; set; }
    public bool WithDeleted { get; set; } = false;
}