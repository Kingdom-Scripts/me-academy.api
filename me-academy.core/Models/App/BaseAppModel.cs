using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Models.App;

public class BaseAppModel
{
    // [PrimaryKey("Id")] // TODO
    [Required]
    public int Id { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}