using System.Text;
using Mapster;
using me_academy.core.Constants;
using me_academy.core.Interfaces;
using me_academy.core.Models.ApiVideo.Request;
using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.App;
using me_academy.core.Models.Configurations;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Videos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace me_academy.core.Services;

public class VideoService : IVideoService
{
    private readonly HttpClient _client;
    private readonly ApiVideoConfig _apiVideoConfig;
    private readonly MeAcademyContext _context;

    public VideoService(IHttpClientFactory factory, IOptions<AppConfig> appConfig, MeAcademyContext context)
    {
        if (appConfig is null) throw new ArgumentNullException(nameof(appConfig));
        if (factory is null) throw new ArgumentNullException(nameof(factory));
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _apiVideoConfig = appConfig.Value.ApiVideo;
        _client = factory.CreateClient(HttpClientKeys.ApiVideo);
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

    public async Task<Result> SetVideoDetails(string courseUid, VideoDetailModel model)
    {
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .Include(course => course.CourseVideo)
            .Select(c => new Course
            {
                Id = c.Id,
                CourseVideo = c.CourseVideo
            })
            .FirstOrDefaultAsync();

        if (course is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        model.playerId = _apiVideoConfig.PlayerId;
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync($"videos/{model.videoId}", content);
        if (!response.IsSuccessStatusCode)
            return new ErrorResult("Failed to set video details.");

        course.CourseVideo!.VideoId = model.videoId;
        course.CourseVideo.IsUploaded = true;
        course.CourseVideo.VideoDuration = model.Duration;

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult()
            : new ErrorResult("Failed to save video details to server.");
    }

    public async Task<Result> SetVideoPreview(string courseUid, ApiVideoClipModel model)
    {
        var courseVideo = await _context.CourseVideos
            .Where(cv => cv.Course!.Uid == courseUid)
            .Include(cv => cv.Course)
            .FirstOrDefaultAsync();

        if (courseVideo is null || !courseVideo.IsUploaded)
            return new ErrorResult(StatusCodes.Status404NotFound, "Video information not found.");

        var request = new
        {
            source = courseVideo.VideoId!,
            title = courseVideo.Course!.Title,
            @public = true,
            mp4Support = false,
            playerId = _apiVideoConfig.PlayerId,
            clip = new
            {
                startTimecode = model.startTimeCode,
                endTimecode = model.endTimeCode
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("videos", content);
        if (!response.IsSuccessStatusCode)
        {
            var contentString = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<object>(contentString);
            return new ErrorResult("Failed to set video thumbnail.");
        }

        string responseContent = await response.Content.ReadAsStringAsync();
        var apiResult = JsonConvert.DeserializeObject<ApiVideoDetail>(responseContent);

        courseVideo.PreviewVideoId = apiResult!.VideoId;
        courseVideo.PreviewThumbnailUrl = apiResult!.Assets.Thumbnail;

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult()
            : new ErrorResult("Failed to save video thumbnail to server.");
    }

    public async Task<Result> GetVideoPlayerDetails(string courseUid)
    {
        var courseVideo = await _context.CourseVideos
            .Where(cv => cv.Course!.Uid == courseUid)
            .FirstOrDefaultAsync();

        if (courseVideo is null || !courseVideo.IsUploaded)
            return new ErrorResult(StatusCodes.Status404NotFound, "Video information not found.");

        string videoId = courseVideo.VideoId!;
        if (string.IsNullOrWhiteSpace(videoId))
            return new ErrorResult("Video not uploaded.");

        var response = await _client.GetAsync($"videos/{videoId}");
        if (!response.IsSuccessStatusCode)
            return new ErrorResult("Failed to get video details.");

        string content = await response.Content.ReadAsStringAsync();
        var apiResult = JsonConvert.DeserializeObject<ApiVideoDetail>(content);

        // extract the token from the result
        string token = apiResult!.Assets.Player.Split("?token=").Last();

        var result = new VideoDetailView
        {
            VideoId = videoId,
            Token = token
        };

        return new SuccessResult(result);
    }
}