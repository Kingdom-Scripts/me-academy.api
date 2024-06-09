using me_academy.core.Models.Input.Users;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IUserService
{
    Task<Result> UserProfile();
    Task<Result> UpdateProfile(ProfileModel model);
    Task<Result> InviteUser(UserInvitationModel model);
    Task<Result> AcceptInvitation(AcceptInvitationModel model);
    Task<Result> ListInvitedUsers();
    Task<Result> ResendInvitationEmail(int id);
}
