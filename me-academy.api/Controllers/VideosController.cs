using me_academy.core.Interfaces;
using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/videos")]
[Authorize]
public class VideosController : BaseController
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService) => _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));

    /// <summary>
    /// Get video upload token
    /// </summary>
    /// <returns></returns>
    [HttpGet("upload-token")]
    public async Task<IActionResult> GetUploadToken([FromQuery] int expiresInSec = 0)
    {
        var res = await _videoService.GetUploadToken(expiresInSec);
        if (res.Success)
        {
            return ProcessResponse(new SuccessResult(res.Content));
        }

        return ProcessResponse(new ErrorResult(res.Message));
    }

    /// <summary>
    /// Get video upload data
    /// </summary>
    /// <param name="courseUid"></param>
    /// <returns></returns>
    [HttpGet("{courseUid}/upload-data")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<ApiVideoToken>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> GetVideoUploadData(string courseUid)
    {
        var res = await _videoService.GetVideoUploadData(courseUid);
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