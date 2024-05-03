using me_academy.core.Interfaces;
using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/videos")]
public class VideosController : BaseController
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService) => _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));

    /// <summary>
    /// Get video upload data
    /// </summary>
    /// <param name="courseUid"></param>
    /// <returns></returns>
    [HttpGet("{courseUid}/upload-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<ApiVideoToken>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> GetVideoUploadData(string courseUid)
    {
        var res = await _videoService.GetVideoUploadData(courseUid);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Update the details of the video after successful upload to Api.Video
    /// </summary>
    /// <param name="courseUid"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch("{courseUid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> SetVideoDetails(string courseUid, VideoDetailModel model)
    {
        var res = await _videoService.SetVideoDetails(courseUid, model);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Set the preview video for a course
    /// </summary>
    /// <param name="courseUid"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch("{courseUid}/preview")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> SetVideoPreview(string courseUid, ApiVideoClipModel model)
    {
        var res = await _videoService.SetVideoPreview(courseUid, model);
        return ProcessResponse(res);
    }
}