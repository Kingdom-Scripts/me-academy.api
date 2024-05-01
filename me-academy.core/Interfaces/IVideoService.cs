using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IVideoService
{
    Task<Result<ApiVideoToken>> CreateUploadObject();
    Task<Result> SetVideoDetails(string courseUid, VideoDetailModel model);
}