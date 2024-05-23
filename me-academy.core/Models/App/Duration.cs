using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class Duration : BaseAppModel
{
    [MaxLength(255)]
    public required string Name { get; set; }
    public short Count { get; set; }
    [MaxLength(15)]
    public required string Type { get; set; }
}