using me_academy.core.Models.App.Constants;

namespace me_academy.core.Models.Input.Auth;

public class UserSession
{
    public int UserId { get; set; }
    public string Uid { get; set; } = null!;
    public string Type { get; set; } = null!;

    private List<string> _roles = new();

    public List<string> Roles
    {
        set => _roles = value;
    }

    public bool InRole(params string[] roles)
    {
        return roles.Any(role => _roles.Contains(role));
    }

    public bool IsAnyAdmin => InRole(RolesConstants.SuperAdmin, RolesConstants.Admin);
    public bool IsCustomer => InRole(RolesConstants.Customer);
    public bool IsSuperAdmin => InRole(RolesConstants.SuperAdmin);
    public bool IsAdmin => InRole(RolesConstants.Admin);
}