using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IConfigService
{
    Task<Result> ListDurations();
}