using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IUserService
{
    Task<Result> UserProfile();
}
