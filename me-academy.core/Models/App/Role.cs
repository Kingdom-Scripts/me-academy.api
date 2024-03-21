using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class Role
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}