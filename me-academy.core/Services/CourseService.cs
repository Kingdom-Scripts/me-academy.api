using Mapster;
using me_academy.core.Extensions;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Courses;
using me_academy.core.Models.Input.Videos;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Courses;
using me_academy.core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace me_academy.core.Services;
public class CourseService : ICourseService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IVideoService _videoService;

    public CourseService(MeAcademyContext context, UserSession userSession, IFileService fileService,
        IHttpContextAccessor httpContextAccessor, IVideoService videoService)
    {
        _context = context ?? throw new ArgumentException(nameof(context));
        _userSession = userSession ?? throw new ArgumentException(nameof(userSession));
        _fileService = fileService ?? throw new ArgumentException(nameof(fileService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        _videoService = videoService ?? throw new ArgumentException(nameof(videoService));
    }

    #region Courses

    public async Task<Result> CreateCourse(CourseModel model)
    {
        // validate that course with title doesn't exist
        bool courseExist = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .AnyAsync(c => c.Title.ToLower().Trim() == model.Title.ToLower().Trim());

        if (courseExist)
            return new ErrorResult("A course with this title already exist. Please choose another title.");

        // create course object
        var course = model.Adapt<Course>();
        course.CreatedById = _userSession.UserId;
        course.Uid = await GetCourseUid(model.Title);

        // add prices
        if (model.Prices.Any())
        {
            course.CoursePrices = model.Prices.Select(p => new CoursePrice
            {
                Price = p.Price,
                DurationId = p.DurationId
            }).ToList();
        }

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Created(course.Title, _userSession.Name));

        // save the data
        await _context.AddAsync(course);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status201Created, course.Adapt<CourseView>())
            : new ErrorResult("Failed to create course. Please try again.");
    }

    public async Task<Result> UpdateCourse(string courseUid, CourseModel model)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .Include(c => c.CoursePrices)
            .Include(c => c.UsefulLinks)
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();

        if (course == null)
            return new ErrorResult("Course not found.");

        // validate that course with title doesn't exist
        bool courseExist = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .AnyAsync(c => c.Id != course.Id
                           && c.Title.ToLower().Trim() == model.Title.ToLower().Trim());

        if (courseExist)
            return new ErrorResult("A course with this title already exist. Please choose another title.");

        // update the course object
        course = model.Adapt(course);
        course.Uid = await GetCourseUid(model.Title);
        course.UpdatedById = _userSession.UserId;
        course.UpdatedOnUtc = DateTime.UtcNow;
        course.UsefulLinks = new();

        // update the IsDeleted for the removed prices
        var removedPrices = course.CoursePrices
            .Where(cp => model.Prices.All(p => p.DurationId != cp.DurationId))
            .ToList();

        _context.CoursePrices.RemoveRange(removedPrices);

        // add new prices
        foreach (var price in model.Prices)
        {
            var existingPrice = course.CoursePrices.FirstOrDefault(cp => cp.DurationId == price.DurationId);
            if (existingPrice != null)
            {
                existingPrice.Price = price.Price;
                existingPrice.IsDeleted = false;
            }
            else
            {
                course.CoursePrices.Add(new CoursePrice
                {
                    Price = price.Price,
                    DurationId = price.DurationId
                });
            }
        }

        // remove the already deleted useful links
        var removedLinks = course.UsefulLinks
            .Where(ul => model.UsefulLinks.All(l => ul.Title != l.Title && ul.Link != l.Link));
        _context.CourseLinks.RemoveRange(removedLinks);

        // add new links
        foreach (var link in model.UsefulLinks)
        {
            var existingLink = course.UsefulLinks.FirstOrDefault(ul => ul.Title == link.Title && ul.Link == link.Link);
            if (existingLink is null)
            {
                course.UsefulLinks.Add(new CourseLink
                {
                    Title = link.Title,
                    Link = link.Link
                });
            }
        }

        // save the data
        _context.Update(course);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(course.Adapt<CourseDetailView>())
            : new ErrorResult("Failed to update course. Please try again.");
    }

    public async Task<Result> DeleteCourse(string courseUid)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        // delete the course
        _context.Remove(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Deleted(course.Title, _userSession.Name));

        int deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course deleted successfully.")
            : new ErrorResult("Failed to delete course. Please try again.");
    }

    public async Task<Result> GetCourse(string courseUid)
    {
        var course = _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .AsQueryable();

        // include deleted ones if user is admin
        if (!_userSession.IsAnyAdmin)
            course = course.Where(c => !c.IsDeleted && c.IsActive && c.IsPublished);

        var result = await course
            .Where(c => c.Uid == courseUid)
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
            .Include(c => c.DeletedBy)
            .Include(c => c.UsefulLinks)
            .Include(c => c.CoursePrices)
            .ThenInclude(cp => cp.Duration)
            .ProjectToType<CourseDetailView>()
            .FirstOrDefaultAsync();

        if (result is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        // remove deleted prices
        result.CoursePrices = result.CoursePrices.Where(cp => !cp.IsDeleted).ToList();

        result.Resources = _context.CourseDocuments
            .Where(cd => cd.CourseId == result.Id)
            .Select(cd => cd.Document)
            .ProjectToType<DocumentView>();

        return new SuccessResult(result);
    }

    public async Task<Result> AddCourseView(string courseUid)
    {
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        course.ViewCount++;
        _context.Update(course);

        var countDetail = new CourseViewCount
        {
            CourseId = course.Id,
            ViewedById = _userSession.UserId != 0 ? _userSession.UserId : null,
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
        };
        await _context.AddRangeAsync(countDetail);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            Log.Error(
                "An error occurred while updating course view count for course: {CourseTitle}. Current view count before failure: {Count}",
                course.Title, course.ViewCount - 1);

        return saved > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course view count updated successfully.")
            : new ErrorResult("Failed to update course view count. Please try again.");
    }

    public async Task<Result> ListCourses(CourseSearchModel request)
    {
        if ((request.IsDraft || request.IsActive || request.WithDeleted) && (!_userSession.IsAnyAdmin && !_userSession.InRole(RolesConstants.ManageCourse)))
            return new ForbiddenResult();

        request.SearchQuery = !string.IsNullOrEmpty(request.SearchQuery)
            ? request.SearchQuery.ToLower().Trim()
            : null;

        var courses = _context.Courses.Where(c => !c.ForSeriesOnly).AsQueryable();

        // allow filters only for admin users or users who can manage courses
        courses = _userSession.IsAnyAdmin || _userSession.InRole(RolesConstants.ManageCourse)
            ? courses
                .Where(c => c.IsActive == request.IsActive && c.IsDraft == request.IsDraft)
                .Where(c => request.WithDeleted || !c.IsDeleted)
            : courses.Where(c => !c.IsDeleted && !c.IsDraft && c.IsActive && c.IsPublished);

        // TODO: implement Full text search for description and title
        var result = await courses
            .Where(c => string.IsNullOrEmpty(request.SearchQuery)
                        || c.Title.ToLower().Contains(request.SearchQuery))
            .ProjectToType<CourseView>()
            .ToPaginatedListAsync(request.PageIndex, request.PageSize);

        return new SuccessResult(result);
    }

    public async Task<Result> PublishCourse(string courseUid)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        if (course.IsPublished)
            return new ErrorResult("Course is already published.");

        // TODO: validate the course has video already before publishing

        course.IsPublished = true;
        course.PublishedOnUtc = DateTime.UtcNow;
        course.PublishedById = _userSession.UserId;

        _context.Update(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Published(course.Title, _userSession.Name));

        int published = await _context.SaveChangesAsync();

        return published > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course published successfully.")
            : new ErrorResult("Failed to publish course. Please try again.");
    }

    public async Task<Result> ActivateCourse(string courseUid)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        course.IsActive = true;

        _context.Update(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Activated(course.Title, _userSession.Name));

        int activated = await _context.SaveChangesAsync();

        return activated > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course activated successfully.")
            : new ErrorResult("Failed to activate course. Please try again.");
    }

    public async Task<Result> DeactivateCourse(string courseUid)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        course.IsActive = false;

        _context.Update(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Deactivated(course.Title, _userSession.Name));

        int deactivated = await _context.SaveChangesAsync();

        return deactivated > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course deactivated successfully.")
            : new ErrorResult("Failed to deactivate course. Please try again.");
    }

    #endregion

    #region Resources

    public async Task<Result> SetCourseVideoDetails(string courseUid, VideoDetailModel model)
    {
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
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

        var detailsSet = await _videoService.SetVideoDetails(model);
        if (!detailsSet.Success)
            return detailsSet;

        course.CourseVideo!.VideoId = model.videoId;
        course.CourseVideo.IsUploaded = true;
        course.CourseVideo.VideoDuration = model.Duration;

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult()
            : new ErrorResult("Failed to save video details to server.");
    }

    public async Task<Result> AddResourceToCourse(string courseUid, FileUploadModel model)
    {
        // validate course
        var course = await _context.Courses
            .Where(c => !c.ForSeriesOnly)
            .Where(c => c.Uid == courseUid)
            .Select(c => new Course
            {
                Id = c.Id,
                Uid = c.Uid,
                Title = c.Title
            })
            .FirstOrDefaultAsync();

        if (course is null)
            return new ErrorResult("Invalid course selected.");

        // save the file
        var fileSaved = await _fileService.UploadFileInternal(course.Uid, model.File);
        if (!fileSaved.Success)
            return new ErrorResult(fileSaved.Title, fileSaved.Message);

        var newCourseDoc = new CourseDocument
        {
            CourseId = course.Id,
            Document = fileSaved.Content,
            CreatedById = _userSession.UserId
        };

        await _context.AddAsync(newCourseDoc);

        // add audit log
        AddCourseAuditLog(course,
            CourseAuditLogConstants.AddResource(course.Title, _userSession.Name, fileSaved.Content.Name));

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status201Created, fileSaved.Content.Adapt<DocumentView>())
            : new ErrorResult("Failed to add resource to course. Please try again.");
    }

    public async Task<Result> RemoveResourceFromCourse(string courseUid, int documentId)
    {
        // validate document
        var courseDoc = await _context.CourseDocuments
            .Where(cd => cd.Course!.Uid == courseUid && cd.DocumentId == documentId)
            .Include(cd => cd.Course)
            .Include(cd => cd.Document)
            .FirstOrDefaultAsync();

        if (courseDoc is null)
            return new ErrorResult("Invalid resource.");

        // delete the file
        var fileDeleted = await _fileService.DeleteFileInternal(documentId);
        if (!fileDeleted.Success)
            return new ErrorResult(fileDeleted.Title, fileDeleted.Message);

        // remove the course document
        _context.Remove(courseDoc.Document!);
        _context.Remove(courseDoc);

        // add audit log
        AddCourseAuditLog(courseDoc.Course!,
            CourseAuditLogConstants.RemoveResource(courseDoc.Course!.Title, _userSession.Name,
                courseDoc.Document!.Name));

        int deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? new SuccessResult("Resource removed successfully.")
            : new ErrorResult("Failed to remove resource from course. Please try again.");
    }

    public async Task<Result> ListResources(string courseUid)
    {
        var resources = await _context.CourseDocuments
            .Where(cd => cd.Course!.Uid == courseUid)
            .Select(cd => cd.Document)
            .ProjectToType<DocumentView>()
            .ToListAsync();

        return new SuccessResult(resources);
    }

    #endregion

    #region Private Methods

    private async Task<string> GetCourseUid(string title)
    {
        var trimmedTitle = title.Trim()  // trim
            .ToLower().Replace("-", "", StringComparison.OrdinalIgnoreCase) // remove hyphens
            .Replace(" ", "-", StringComparison.OrdinalIgnoreCase); // replace spaces with hyphens

        // get the next course number from sequence
        var nextCourseNumber = await _context.GetNextCourseNumber();
        return $"{trimmedTitle}-{nextCourseNumber}";
    }

    private async void AddCourseAuditLog(Course course, string description)
    {
        var newLog = new CourseAuditLog
        {
            Course = course,
            Description = description,
            CreatedById = _userSession.UserId
        };
        await _context.AddAsync(newLog);
    }

    #endregion
}