using me_academy.core.Models.App;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Courses;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Services;

public class DashbaordService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;

    public DashbaordService(MeAcademyContext context, UserSession userSession)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
    }

    //public async Task<Result> ListAll
}
