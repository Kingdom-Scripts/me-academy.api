using System.Text.Json.Serialization;

namespace me_academy.core.Models.View.Courses;

public class CoursePriceView
{
    public int DurationId   { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
}