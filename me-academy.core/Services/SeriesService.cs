﻿using Mapster;
using me_academy.core.Extensions;
using me_academy.core.Interfaces;
using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Series;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View;
using me_academy.core.Models.View.Series;
using me_academy.core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace me_academy.core.Services;

public class SeriesService : ISeriesService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly IVideoService _videoService;

    public SeriesService(MeAcademyContext context, UserSession userSession, IVideoService videoService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
        _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
    }

    public async Task<Result> CreateSeries(SeriesModel model)
    {
        bool seriesExists = await _context.Series
            .AnyAsync(x => x.Title.ToLower().Trim() == model.Title.ToLower().Trim());

        if (seriesExists)
            return new ErrorResult("Series with the same title already exists.");

        // create new series object
        var series = model.Adapt<Series>();
        series.CreatedById = _userSession.UserId;
        series.Uid = await GetSeriesUid(model.Title);
        series.Preview = new()
        {
            UploadToken = ""
        };

        // add prices
        if (model.Prices.Any())
        {
            series.SeriesPrices = model.Prices.Select(p => new SeriesPrice
            {
                Price = p.Price,
                DurationId = p.DurationId
            }).ToList();
        }

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Created(series.Title, _userSession.Name));

        // save the data
        await _context.AddAsync(series);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status201Created, series.Adapt<SeriesView>())
            : new ErrorResult("Failed to create series.");
    }

    public async Task<Result> UpdateSeries(string seriesUid, SeriesModel model)
    {
        var series = await _context.Series
            .Include(x => x.SeriesPrices)
            .FirstOrDefaultAsync(x => x.Uid == seriesUid);

        if (series == null)
            return new ErrorResult("Series not found.");

        // update series object
        series.Title = model.Title;
        series.Uid = await GetSeriesUid(model.Title);
        series.UpdatedById = _userSession.UserId;
        series.UpdatedOnUtc = DateTime.UtcNow;

        // remove already deleted prices
        var removedPrices = series.SeriesPrices
            .Where(x => model.Prices.All(p => p.DurationId != x.DurationId));
        _context.RemoveRange(removedPrices);

        // add new prices
        foreach (var price in model.Prices)
        {
            if (series.SeriesPrices.Any(x => x.DurationId == price.DurationId))
            {
                series.SeriesPrices.First(x => x.DurationId == price.DurationId).Price = price.Price;
            }
            else
            {
                series.SeriesPrices.Add(new SeriesPrice
                {
                    Price = price.Price,
                    DurationId = price.DurationId
                });
            }
        }

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Updated(series.Title, _userSession.Name));

        // save the data
        _context.Update(series);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, series.Adapt<SeriesView>())
            : new ErrorResult("Failed to update series.");
    }

    public async Task<Result> DeleteSeries(string seriesUid)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Uid == seriesUid);

        if (series == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series not found.");

        // delete the series
        _context.Remove(series);

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Deleted(series.Title, _userSession.Name));

        // save the data
        _context.Update(series);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, "S")
            : new ErrorResult("Failed to delete series.");
    }

    public async Task<Result> ListSeries(SeriesSearchModel request)
    {
        if ((request.IsDraft || request.IsActive || request.WithDeleted) && !_userSession.IsAnyAdmin)
            return new ForbiddenResult();

        request.SearchQuery = !string.IsNullOrWhiteSpace(request.SearchQuery)
            ? request.SearchQuery.ToLower().Trim()
            : null;

        var series = _context.Series.AsQueryable();

        // allow filters only for admin users or users who can manage courses
        series = _userSession.IsAnyAdmin || _userSession.InRole(RolesConstants.ManageCourse)
            ? series.Where(s => s.IsActive == request.IsActive && s.IsDraft == request.IsDraft)
                .Where(s => request.WithDeleted || !s.IsDeleted)
            : series.Where(s => s.IsActive && s.IsPublished && !s.IsDeleted);

        var result = await series
            .Where(s => string.IsNullOrWhiteSpace(request.SearchQuery) ||
                        s.Title.ToLower().Contains(request.SearchQuery))
            .ProjectToType<SeriesView>()
            .ToPaginatedListAsync(request.PageIndex, request.PageSize);

        return new SuccessResult(result);
    }

    public async Task<Result> GetSeries(string seriesUid)
    {
        var series = _context.Series.AsQueryable();

        // include deleted ones if user is admin
        if (!_userSession.IsAnyAdmin)
            series = series.Where(s => !s.IsDeleted && s.IsActive && s.IsPublished);

        var result = await series
            .Where(s => s.Uid == seriesUid)
            .ProjectToType<SeriesDetailView>()
            .FirstOrDefaultAsync();

        if (result is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series is not found.");

        // remove the deleted prices
        result.SeriesPrices = result.SeriesPrices.Where(x => !x.IsDeleted).ToList();

        return new SuccessResult(result);
    }

    public async Task<Result> AddSeriesView(string seriesUid)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Uid == seriesUid);

        if (series == null)
            return new ErrorResult("Series not found.");

        series.ViewCount++;
        _context.Update(series);
        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            Log.Error(
                "An error occurred while updating series view count for series: {SeriesTitle}. Current view count before failure: {Count}",
                series.Title, series.ViewCount - 1);

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Series view count updated successfully.")
            : new ErrorResult("Failed to add view.");
    }

    public async Task<Result> PublishSeries(string seriesUid)
    {
        var series = await _context.Series
            .Where(s => s.Uid == seriesUid)
            .Include(s => s.Preview)
            .FirstOrDefaultAsync();

        if (series is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series is not found.");

        if (series.IsPublished)
            return new ErrorResult("Series is already published.");

        // validate that series set up is complete
        if (string.IsNullOrWhiteSpace(series.Preview!.VideoId))
            return new ErrorResult("Series preview video is not set.");

        series.IsPublished = true;
        series.PublishedOnUtc = DateTime.UtcNow;
        series.PublishedById = _userSession.UserId;

        _context.Update(series);

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Published(series.Title, _userSession.Name));

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Series published successfully.")
            : new ErrorResult("Failed to publish series.");
    }

    public async Task<Result> ActivateSeries(string seriesUid)
    {
        var series = await _context.Series
            .Where(s => s.Uid == seriesUid)
            .FirstOrDefaultAsync();

        if (series is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series is not found.");

        series.IsActive = true;

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Activated(series.Title, _userSession.Name));

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Series activated successfully.")
            : new ErrorResult("Failed to activate series.");
    }

    public async Task<Result> DeactivateSeries(string seriesUid)
    {
        var series = await _context.Series
            .Where(s => s.Uid == seriesUid)
            .FirstOrDefaultAsync();

        if (series is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series is not found.");

        series.IsActive = false;

        // add audit log
        AddSeriesAuditLog(series, SeriesAuditLogConstants.Deactivated(series.Title, _userSession.Name));

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Series deactivated successfully.")
            : new ErrorResult("Failed to deactivate series.");
    }

    public async Task<Result> GetUploadToken(string seriesUid)
    {
        var uploadToken = _context.SeriesPreviews
            .Where(cv => cv.Series!.Uid == seriesUid)
            .Select(cv => new ApiVideoToken { Token = cv.UploadToken })
            .FirstOrDefault();

        if (uploadToken is null || string.IsNullOrEmpty(uploadToken.Token))
        {
            var uploadTokenRes = await _videoService.GetUploadToken();
            if (!uploadTokenRes.Success)
                return new ErrorResult(uploadTokenRes.Message);

            uploadToken = uploadTokenRes.Content;

            int seriesId = await _context.Series
                .Where(c => c.Uid == seriesUid)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            var newUploadData = new SeriesPreview
            {
                SeriesId = seriesId,
                UploadToken = uploadToken!.Token
            };
            await _context.AddAsync(newUploadData);
            await _context.SaveChangesAsync();
        }

        return new SuccessResult(uploadToken);
    }

    public async Task<Result> SetPreviewDetails(string seriesUid, VideoDetailModel model)
    {
        var series = await _context.Series
            .Where(c => c.Uid == seriesUid)
            .Include(series => series.Preview)
            .Select(c => new Series
            {
                Id = c.Id,
                Preview = c.Preview
            })
            .FirstOrDefaultAsync();

        if (series is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series not found.");

        // delete existing video
        if (!string.IsNullOrEmpty(series.Preview!.VideoId))
        {
            var deletedRes = await _videoService.DeleteVideo(series.Preview.VideoId);
            if (!deletedRes.Success)
                Log.Error("Failed to delete existing video for series: {SeriesUid}, videoId: {VideoId}", seriesUid, series.Preview.VideoId);
        }

        model.@public = true;
        var detailsSet = await _videoService.SetVideoDetails(model);
        if (!detailsSet.Success)
            return detailsSet;

        series.Preview!.VideoId = model.videoId;
        series.Preview.ThumbnailUrl = model.ThumbnailUrl;
        series.Preview.IsUploaded = true;
        series.Preview.VideoDuration = model.Duration;

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult()
            : new ErrorResult("Failed to save video details to server.");
    }

    public async Task<Result> GetPreviewDetails(string seriesUid)
    {
        var previewDetails = await _context.SeriesPreviews
            .Where(cv => cv.Series!.Uid == seriesUid)
            .ProjectToType<VideoView>()
            .FirstOrDefaultAsync();

        if (previewDetails is null)
            return new SuccessResult(StatusCodes.Status204NoContent, "Preview details not found.");

        return new SuccessResult(previewDetails);
    }

    public async Task<Result> AddExistingCourseToSeries(string seriesUid, string courseUid)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Uid == seriesUid);

        if (series is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series not found.");

        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .Select(c => new Course { Id = c.Id, IsActive = c.IsActive, Title =c.Title, Summary = c.Summary})
            .FirstOrDefaultAsync();

        if (course is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");
        if (!course.IsActive)
            return new ErrorResult("Cannot add a deactivated course.");

        bool courseExistInSeries = await _context.SeriesCourses
            .AnyAsync(sc => sc.SeriesId == series.Id && sc.CourseId == course.Id && !sc.IsDeleted);

        if (courseExistInSeries)
            return new ErrorResult("Course already exists in this series.");

        var seriesCourse = new SeriesCourse
        {
            SeriesId = series.Id,
            CourseId = course.Id,
            Order = _context.SeriesCourses.Count(sc => sc.SeriesId == series.Id && !sc.IsDeleted) + 1,
            CreatedById =  _userSession.UserId
        };

        await _context.AddAsync(seriesCourse);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Failed to save changes.");

        seriesCourse.Course = course;
        return new SuccessResult(seriesCourse.Adapt<SeriesCourseView>());
    }

    public async Task<Result> AddNewCourseToSeries(string seriesUid, SeriesNewCourseModel model)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Uid == seriesUid);

        if (series is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Series not found.");

        var newCourse = model.Adapt<Course>();
        newCourse.CreatedById = _userSession.UserId;
        newCourse.Uid = await GetCourseUid(model.Title);
        newCourse.ForSeriesOnly = true;
        newCourse.IsPublished = true;
        newCourse.IsDraft = false;

        // set video details
        var detailsSet = await _videoService.SetVideoDetails(model.VideoDetails);
        if (!detailsSet.Success)
            return detailsSet;

        newCourse.CourseVideo = new CourseVideo
        {
            UploadToken = "",
            VideoId = model.VideoDetails.videoId,
            VideoDuration = model.VideoDetails.Duration,
            IsUploaded = true
        };

        var seriesCourse = new SeriesCourse
        {
            SeriesId = series.Id,
            Course = newCourse,
            Order = _context.SeriesCourses.Count() + 1
        };

        await _context.AddAsync(seriesCourse);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(seriesCourse.Adapt<SeriesCourseView>())
            : new ErrorResult("Failed to save changes.");
    }

    public async Task<Result> RemoveCourseFromSeries(string seriesUid, string seriesCourseId)
    {
        var seriesCourse = await _context.SeriesCourses
            .FirstOrDefaultAsync(sc => sc.Course!.Uid == seriesCourseId && sc.Series!.Uid == seriesUid && !sc.IsDeleted);

        if (seriesCourse is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found in this series.");

        _context.Remove(seriesCourse);

        // get courses after this one
        var remainingCourses = await _context.SeriesCourses
            .Where(sc => sc.SeriesId == seriesCourse.SeriesId && sc.Order > seriesCourse.Order)
            .ToListAsync();

        // decrement the order of the remaining courses
        foreach (var course in remainingCourses)
        {
            course.Order--;
            _context.Update(course);
        }

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult()
            : new ErrorResult("Failed to delete series course.");
    }

    public async Task<Result> ListCoursesInSeries(string seriesUid)
    {
        var seriesCourses = await _context.SeriesCourses
            .Include(sc => sc.Course)
            .Where(sc => sc.Series!.Uid == seriesUid && !sc.IsDeleted)
            .OrderBy(sc => sc.Order)
            .ProjectToType<SeriesCourseView>()
            .ToListAsync();

        return new SuccessResult(seriesCourses);
    }

    public async Task<Result> ChangeCourseOrder(string seriesUid, string courseUid, SeriesCourseOrderModel model)
    {
        var seriesCourse = await _context.SeriesCourses
            .Where(sc => sc.Series!.Uid == seriesUid && sc.Course!.Uid == courseUid && !sc.IsDeleted)
            .FirstOrDefaultAsync();
        if (seriesCourse is null)
            return new ErrorResult("Course not found in this series.");

        // Get the current order of the course
        int currentOrder = seriesCourse.Order;

        // Update the order of the specified course
        seriesCourse.Order = model.NewOrder;
        seriesCourse.UpdatedById = _userSession.UserId;
        seriesCourse.UpdatedOnUtc = DateTime.UtcNow;

        // Adjust the order of the other courses in the series
        var otherCourses = await _context.SeriesCourses
            .Where(sc => sc.Series!.Uid == seriesUid && sc.Course!.Uid != courseUid && !sc.IsDeleted)
            .ToListAsync();

        foreach (var course in otherCourses)
        {
            if (model.NewOrder < currentOrder)
            {
                if (course.Order >= model.NewOrder && course.Order < currentOrder)
                    course.Order++;
            }
            else
            {
                if (course.Order <= model.NewOrder && course.Order > currentOrder)
                    course.Order--;
            }

            course.UpdatedById = _userSession.UserId;
            course.UpdatedOnUtc = DateTime.UtcNow;
            _context.Update(course);
        }

        _context.Update(seriesCourse);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult()
            : new ErrorResult("Failed to save changes.");
    }

    #region PRIVATE METHODS

    private async Task<string> GetSeriesUid(string title)
    {
        var trimmedTitle = title.Trim() // trim
            .ToLower().Replace("-", "", StringComparison.OrdinalIgnoreCase) // remove hyphens
            .Replace(" ", "-", StringComparison.OrdinalIgnoreCase); // replace spaces with hyphens

        // get the next series number from sequnce
        var nextSeriesNumber = await _context.GetNextSeriesNumber();
        return $"{trimmedTitle}-{nextSeriesNumber}";
    }

    private async Task<string> GetCourseUid(string title)
    {
        var trimmedTitle = title.Trim()  // trim
            .ToLower().Replace("-", "", StringComparison.OrdinalIgnoreCase) // remove hyphens
            .Replace(" ", "-", StringComparison.OrdinalIgnoreCase); // replace spaces with hyphens

        // get the next course number from sequence
        var nextCourseNumber = await _context.GetNextCourseNumber();
        return $"{trimmedTitle}-{nextCourseNumber}";
    }

    private async void AddSeriesAuditLog(Series Series, string description)
    {
        var newLog = new SeriesAuditLog
        {
            Series = Series,
            Description = description,
            CreatedById = _userSession.UserId
        };
        await _context.AddAsync(newLog);
    }

    #endregion
}