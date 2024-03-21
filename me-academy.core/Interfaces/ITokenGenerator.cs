using me_academy.core.Models.App;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface ITokenGenerator
{
    Task<Result> GenerateJwtToken(User user);

    Task<Result> RefreshJwtToken(string refreshToken);

    Task InvalidateToken(string userReference);
}