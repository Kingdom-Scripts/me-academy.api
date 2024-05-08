using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class SeriesLink
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    [MaxLength(150)]
    public required string Title { get; set; }
    [MaxLength(225)]
    public required string Link { get; set; }
}