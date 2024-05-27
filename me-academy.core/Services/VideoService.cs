using System.Text;
using Mapster;
using me_academy.core.Constants;
using me_academy.core.Interfaces;
using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.App;
using me_academy.core.Models.Configurations;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View;
using me_academy.core.Models.View.Videos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace me_academy.core.Services;

public class VideoService : IVideoService
{
    private readonly HttpClient _client;
    private readonly ApiVideoConfig _apiVideoConfig;
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly ILogger _logger;

    public VideoService(IHttpClientFactory factory, IOptions<AppConfig> appConfig, MeAcademyContext context, UserSession userSession, ILogger logger)
    {
        if (appConfig is null) throw new ArgumentNullException(nameof(appConfig));
        if (factory is null) throw new ArgumentNullException(nameof(factory));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));

        _apiVideoConfig = appConfig.Value.ApiVideo;
        _client = factory.CreateClient(HttpClientKeys.ApiVideo);
    }

    public async Task<Result<ApiVideoToken>> GetUploadToken(int expiresInSec = 0)
    { 
        HttpResponseMessage response = new HttpResponseMessage();
        if (expiresInSec == 0)
        {
            response = await _client.PostAsync("upload-tokens", null);
        }
        else
        {
            var request = new { ttl = expiresInSec };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            response = await _client.PostAsync("upload-tokens", content);
        }

        if (!response.IsSuccessStatusCode)
            return new ErrorResult<ApiVideoToken>("Failed to get video upload token");

        string contentString = await response.Content.ReadAsStringAsync();
        var uploadToken = JsonConvert.DeserializeObject<ApiVideoToken>(contentString);

        return new SuccessResult<ApiVideoToken>(uploadToken!);
    }

    public async Task<Result> GetVideoUploadData(string courseUid)
    {
        var uploadToken = _context.CourseVideos
            .Where(cv => cv.Course!.Uid == courseUid)
            .Select(cv => new ApiVideoToken { Token = cv.UploadToken })
            .FirstOrDefault();

        if (uploadToken is null)
        {
            var response = await _client.PostAsync("upload-tokens", null);
            if (!response.IsSuccessStatusCode)
                return new ErrorResult("Failed to get video upload token");

            string content = await response.Content.ReadAsStringAsync();
            uploadToken = JsonConvert.DeserializeObject<ApiVideoToken>(content);

            int courseId = await _context.Courses
                .Where(c => c.Uid == courseUid)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            var newUploadData = new CourseVideo
            {
                CourseId = courseId,
                UploadToken = uploadToken!.Token
            };
            await _context.AddAsync(newUploadData);
            await _context.SaveChangesAsync();
        }

        return new SuccessResult(uploadToken);
    }

    public async Task<Result> SetVideoDetails(VideoDetailModel model)
    {
        model.playerId = _apiVideoConfig.PlayerId;
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync($"videos/{model.videoId}", content);
        if (response.IsSuccessStatusCode)
        {
            return new SuccessResult();
        }

        string contentString = await response.Content.ReadAsStringAsync();
        object? error = JsonConvert.DeserializeObject<object>(contentString);
        _logger.Error("Failed to set video details. {@Error}", error);
        return new ErrorResult("Failed to set video details.");
    }

    public async Task<Result> SetVideoPreview(string courseUid, ApiVideoClipModel model)
    {
        var courseVideo = await _context.CourseVideos
            .Where(cv => cv.Course!.Uid == courseUid)
            .Include(cv => cv.Course)
            .FirstOrDefaultAsync();

        if (courseVideo is null || !courseVideo.IsUploaded)
            return new ErrorResult(StatusCodes.Status404NotFound, "Source video information not found.");

        // check if there is a preview video already set
        if (!string.IsNullOrWhiteSpace(courseVideo.PreviewVideoId))
        {
            var deleteResponse = await _client.DeleteAsync($"videos/{courseVideo.PreviewVideoId}");
            if (!deleteResponse.IsSuccessStatusCode)
            {
                string contentString = await deleteResponse.Content.ReadAsStringAsync();
                object? error = JsonConvert.DeserializeObject<object>(contentString);
                _logger.Error("Failed to delete video preview. {@Error}", error);
                return new ErrorResult("Failed to delete existing video preview.");
            }
        }

        var request = new
        {
            source = courseVideo.VideoId!,
            title = courseVideo.Course!.Title + "- (Preview)",
            @public = true,
            mp4Support = false,
            playerId = _apiVideoConfig.PlayerId,
            clip = new
            {
                startTimecode = model.startTimecode.ToString(),
                endTimecode = model.endTimecode.ToString()
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("videos", content);
        if (!response.IsSuccessStatusCode)
        {
            string contentString = await response.Content.ReadAsStringAsync();
            object? error = JsonConvert.DeserializeObject<object>(contentString);
            _logger.Error("Failed to set video preview. {@Error}", error);
            return new ErrorResult("Failed to set video thumbnail.");
        }

        string responseContent = await response.Content.ReadAsStringAsync();
        var apiResult = JsonConvert.DeserializeObject<ApiVideoDetail>(responseContent);

        courseVideo.PreviewVideoId = apiResult!.VideoId;
        courseVideo.ThumbnailUrl = apiResult!.Assets.Thumbnail;
        courseVideo.PreviewStart = model.startTimecode;
        courseVideo.PreviewEnd = model.endTimecode;

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(courseVideo.Adapt<VideoView>())
            : new ErrorResult("Failed to save video thumbnail to server.");
    }

    public async Task<Result> DeleteVideo(string videoId)
    {
        var response = await _client.DeleteAsync($"videos/{videoId}");
        if (response.IsSuccessStatusCode)
        {
            return new SuccessResult();
        }

        string contentString = await response.Content.ReadAsStringAsync();
        object? error = JsonConvert.DeserializeObject<object>(contentString);
        _logger.Error("Failed to delete video. {@Error}", error);
        return new ErrorResult("Failed to delete video.");
    }

    // TODO: when implementing this, remember to set up an authentication mechanism
    // to validate that the user actually has paid to see this video or the video is
    // part of a series the user has paid for.
    public async Task<Result> GetVideoPlayerDetails(string courseUid)
    {
        // validate user has paid for course if not a super admin, admin or course manager
        bool shouldHaveUnrestrictedAccess = _userSession.IsAnyAdmin || _userSession.IsCourseManager;
        if (!shouldHaveUnrestrictedAccess)
        {
            var today = DateTime.UtcNow;
            var coursePaid = await _context.UserCourses
                .Where(cp => cp.Course!.Uid == courseUid && cp.UserId == _userSession.UserId)
                .Where(cp => cp.ExpiresAtUtc >= today)
                .AnyAsync();
            if (!coursePaid)
                return new ForbiddenResult();
        }

        var courseVideo = await _context.CourseVideos
            .Where(cv => cv.Course!.Uid == courseUid)
            .FirstOrDefaultAsync();

        if (courseVideo is null || !courseVideo.IsUploaded)
            return new SuccessResult(StatusCodes.Status204NoContent, "Video information not found.");

        string videoId = courseVideo.VideoId!;
        if (string.IsNullOrWhiteSpace(videoId) && !shouldHaveUnrestrictedAccess)
            return new ErrorResult("Video not uploaded.");

        var response = await _client.GetAsync($"videos/{videoId}");
        if (!response.IsSuccessStatusCode)
            return new ErrorResult("Failed to get video details.");

        string content = await response.Content.ReadAsStringAsync();
        var apiResult = JsonConvert.DeserializeObject<ApiVideoDetail>(content);

        // extract the token from the result
        string token = apiResult!.Assets.Player.Split("?token=").Last();

        var result = new VideoView
        {
            VideoId = videoId,
            Token = token,
            PreviewVideoId = courseVideo.PreviewVideoId,
            ThumbnailUrl = courseVideo.ThumbnailUrl,
            VideoDuration = courseVideo.VideoDuration,
            IsUploaded = courseVideo.IsUploaded
        };

        return new SuccessResult(result);
    }
}