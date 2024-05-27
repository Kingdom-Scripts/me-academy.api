using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IVideoService
{
    Task<Result<ApiVideoToken>> GetUploadToken(int expiresInSec = 0);
    Task<Result> GetVideoUploadData(string courseUid);
    Task<Result> SetVideoDetails(VideoDetailModel model);
    Task<Result> SetVideoPreview(string courseUid, ApiVideoClipModel model);
    Task<Result> DeleteVideo(string videoId);
    Task<Result> GetVideoPlayerDetails(string courseUid);
}