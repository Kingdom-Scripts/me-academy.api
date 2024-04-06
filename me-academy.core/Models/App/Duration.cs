using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class DurationType : BaseAppModel
{
    [MaxLength(15)]
    public required string Name { get; set; }
}