using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class InvitedUser : BaseAppModel
{
    [Required]
    [MaxLength(50)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(25)]
    public required string Phone { get; set; }
    public bool CanManageCourses { get; set; }
    public bool CanManageUsers { get; set; }

    public int EmailsSent { get; set; } = 1;
    public int CreatedById { get; set; }
    
    public User? CreatedBy { get; set; }
}