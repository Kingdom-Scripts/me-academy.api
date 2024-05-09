using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IVideoService
{
    Task<Result<ApiVideoToken>> GetUploadToken();
    Task<Result> GetVideoUploadData(string courseUid);
    Task<Result> SetVideoDetails(VideoDetailModel model)
    Task<Result> SetVideoPreview(string courseUid, ApiVideoClipModel model);
}