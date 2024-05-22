using me_academy.core.Models.Input.SmeHub;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface ISmeHubService
{
    Task<Result> CreateSmeHub(SmeHubModel input);
    Task<Result> UpdateSmeHub(string uid, SmeHubModel model);
    Task<Result> RemoveSmeHub(string uid);
    Task<Result> ListSmeHubs(SmeHubSearchModel request);
    Task<Result> GetSmeHub(string uid);
    Task<Result> ListTypes();
    Task<Result> ActivateSmeHub(string uid);
    Task<Result> DeactivateSmeHub(string uid);
}