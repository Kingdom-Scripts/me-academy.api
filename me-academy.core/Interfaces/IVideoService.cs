using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IVideoService
{
    Task<Result> GetVideoUploadData(string courseUid);
    Task<Result> SetVideoDetails(string courseUid, VideoDetailModel model);
    Task<Result> SetVideoPreview(string courseUid, ApiVideoClipModel model);
}