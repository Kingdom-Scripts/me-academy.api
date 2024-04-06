using FluentValidation;
using Mapster;
using me_academy.core.Extensions;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Courses;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Courses;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CourseView = me_academy.core.Models.View.Courses.CourseView;

namespace me_academy.core.Services;

public class CourseService : ICourseService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CourseService> _logger;

    public CourseService(MeAcademyContext context, UserSession userSession, IFileService fileService,
        IHttpContextAccessor httpContextAccessor, ILogger<CourseService> logger)
    {
        _context = context ?? throw new ArgumentException(nameof(context));
        _userSession = userSession ?? throw new ArgumentException(nameof(userSession));
        _fileService = fileService ?? throw new ArgumentException(nameof(fileService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        _logger = logger ?? throw new ArgumentException(nameof(logger));
    }

    public async Task<Result> CreateCourse(CourseModel model)
    {
        // validate that course with title doesn't exist
        bool courseExist = await _context.Courses
            .AnyAsync(c => c.Title.ToLower().Trim() == model.Title.ToLower().Trim());

        if (courseExist)
            return new ErrorResult("A course with this title already exist. Please choose another title.");

        // create course object
        var course = model.Adapt<Course>();
        course.CreatedById = _userSession.UserId;
        course.Uid = course.Title.Trim().ToLower().Replace(" ", "-", StringComparison.OrdinalIgnoreCase);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Created(course.Title, _userSession.Uid));

        // save the data
        await _context.Courses.AddAsync(course);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status201Created, course.Adapt<CourseView>())
            : new ErrorResult("Failed to create course. Please try again.");
    }

    public async Task<Result> AddCourseVideo()
    {
        throw new NotImplementedException();
    }

    public async Task<Result> AddResourceToCourse(string courseUid, IFormFile file)
    {
        // validate course
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .Select(c => new Course
            {
                Id = c.Id
            })
            .FirstOrDefaultAsync();

        if (course is null)
            return new ErrorResult("Invalid course selected.");

        // save the file
        var fileSaved = await _fileService.UploadFileInternal(course.Uid, file);
        if (!fileSaved.Success)
            return new ErrorResult(fileSaved.Title, fileSaved.Message);

        var newCourseDoc = new CourseDocument
        {
            CourseId = course.Id,
            Document = fileSaved.Content
        };

        await _context.AddAsync(newCourseDoc);

        // add audit log
        AddCourseAuditLog(course,
            CourseAuditLogConstants.AddResource(course.Title, _userSession.Uid, fileSaved.Content.Id));

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status201Created, fileSaved.Content.Adapt<DocumentView>())
            : new ErrorResult("Failed to add resource to course. Please try again.");
    }

    public async Task<Result> RemoveResourceFromCourse(string courseUid, int documentId)
    {
        // validate course
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .Select(c => new Course
            {
                Id = c.Id
            })
            .FirstOrDefaultAsync();

        if (course is null)
            return new ErrorResult("Invalid course selected.");

        // validate document
        var courseDoc = await _context.CourseDocuments
            .Where(cd => cd.CourseId == course.Id && cd.DocumentId == documentId)
            .FirstOrDefaultAsync();

        if (courseDoc is null)
            return new ErrorResult("Invalid resource selected.");

        // delete the file
        var fileDeleted = await _fileService.DeleteFileInternal(documentId);
        if (!fileDeleted.Success)
            return new ErrorResult(fileDeleted.Title, fileDeleted.Message);

        // remove the course document
        _context.Remove(courseDoc);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.RemoveResource(course.Title, _userSession.Uid, documentId));

        int deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? new SuccessResult("Resource removed successfully.")
            : new ErrorResult("Failed to remove resource from course. Please try again.");
    }

    public async Task<Result> UpdateCourse(string courseUid, CourseModel model)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();

        if (course == null)
            return new ErrorResult("Course not found.");

        // validate that course with title doesn't exist
        bool courseExist = await _context.Courses
            .AnyAsync(c => c.Id != course.Id
                           && string.Equals(c.Title, model.Title, StringComparison.OrdinalIgnoreCase));

        if (courseExist)
            return new ErrorResult("A course with this title already exist. Please choose another title.");

        // update the course object
        course = model.Adapt(course);
        course.UpdatedById = _userSession.UserId;
        course.UpdatedOnUtc = DateTime.UtcNow;

        // save the data
        _context.Courses.Update(course);
        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(course.Adapt<CourseView>())
            : new ErrorResult("Failed to update course. Please try again.");
    }

    public async Task<Result> DeleteCourse(string courseUid)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        // delete the course
        _context.Courses.Remove(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Deleted(course.Title, _userSession.Uid));

        int deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course deleted successfully.")
            : new ErrorResult("Failed to delete course. Please try again.");
    }

    public async Task<Result> GetCourse(string courseUid)
    {
        var course = _context.Courses.AsQueryable();

        // include deleted ones if user is admin
        if (!_userSession.IsAnyAdmin)
            course = course.Where(c => !c.IsDeleted && c.IsActive);

        var result = await course
            .Where(c => c.Uid == courseUid)
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
            .Include(c => c.DeletedBy)
            .Include(c => c.UsefulLinks)
            .Include(c => c.Resources).ThenInclude(r => r.Document)
            .ProjectToType<CourseDetailView>()
            .FirstOrDefaultAsync();

        if (result is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        // update view count
        UpdateCourseViewCount(courseUid);

        return new SuccessResult(result);
    }

    public async Task<Result> ListCourses(CourseSearchModel request)
    {
        if ((request.IsDraft || request.IsActive || request.WithDeleted) && !_userSession.IsAnyAdmin)
        {
            return new ForbiddenResult();
        }

        request.SearchQuery = !string.IsNullOrEmpty(request.SearchQuery)
            ? request.SearchQuery.ToLower()
            : null;

        var courses = _context.Courses.AsQueryable();

        if (_userSession.IsAnyAdmin && request.WithDeleted)
            courses = courses.Where(c => c.IsDeleted == request.WithDeleted && c.IsActive == request.IsActive);
        else
            courses = courses.Where(c => !c.IsDeleted && c.IsActive);

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
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        if (course is { IsActive: true, PublishedOnUtc: not null })
            return new ErrorResult("Course is already published.");

        course.IsActive = true;
        course.PublishedOnUtc = DateTime.UtcNow;
        course.PublishedById = _userSession.UserId;

        _context.Courses.Update(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Published(course.Title, _userSession.Uid));

        int published = await _context.SaveChangesAsync();

        return published > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course published successfully.")
            : new ErrorResult("Failed to publish course. Please try again.");
    }

    public async Task<Result> ActivateCourse(string courseUid)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        course.IsActive = true;

        _context.Courses.Update(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Activated(course.Title, _userSession.Uid));

        int activated = await _context.SaveChangesAsync();

        return activated > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course activated successfully.")
            : new ErrorResult("Failed to activate course. Please try again.");
    }

    public async Task<Result> DeactivateCourse(string courseUid)
    {
        // get the course
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Course not found.");

        course.IsActive = false;

        _context.Courses.Update(course);

        // add audit log
        AddCourseAuditLog(course, CourseAuditLogConstants.Deactivated(course.Title, _userSession.Uid));

        int deactivated = await _context.SaveChangesAsync();

        return deactivated > 0
            ? new SuccessResult(StatusCodes.Status200OK, "Course deactivated successfully.")
            : new ErrorResult("Failed to deactivate course. Please try again.");
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

    private async void UpdateCourseViewCount(string courseUid)
    {
        var course = await _context.Courses
            .Where(c => c.Uid == courseUid)
            .FirstOrDefaultAsync();
        if (course == null)
            return;

        course.ViewCount++;
        _context.Courses.Update(course);

        var countDetail = new CourseViewCount
        {
            CourseId = course.Id,
            ViewedById = _userSession.UserId != 0 ? _userSession.UserId : null,
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
        };
        await _context.AddRangeAsync(countDetail);

        int saved = await _context.SaveChangesAsync();

        if (saved > 0)
            _logger.LogInformation("Course view count updated for course: {CourseTitle}", course.Title);
        else
            _logger.LogError(
                "An error occurred while updating course view count for course: {CourseTitle}. Current view count before failure: {Count}",
                course.Title, course.ViewCount - 1);
    }
}