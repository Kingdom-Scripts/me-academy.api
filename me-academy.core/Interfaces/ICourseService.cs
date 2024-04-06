using me_academy.core.Models.Input.Courses;
using me_academy.core.Models.Utilities;
using Microsoft.AspNetCore.Http;

namespace me_academy.core.Interfaces;

public interface ICourseService
{
    Task<Result> CreateCourse(CourseModel model);
    Task<Result> AddCourseVideo();
    Task<Result> AddResourceToCourse(string courseUid, IFormFile file);
    Task<Result> RemoveResourceFromCourse(string courseUid, int documentId);
    Task<Result> UpdateCourse(string courseUid, CourseModel model);
    Task<Result> DeleteCourse(string courseUid);
    Task<Result> GetCourse(string courseUid);
    Task<Result> ListCourses(CourseSearchModel request);
    Task<Result> PublishCourse(string courseUid);
    Task<Result> ActivateCourse(string courseUid);
    Task<Result> DeactivateCourse(string courseUid);
}