namespace me_academy.core.Models.App.Constants;

public static class CourseAuditLogConstants
{
    public static string Created(string title, string userUid) => $"Course '{title}' was created by {userUid}";

    public static string Updated(string title, string userUid) => $"Course '{title}' was updated by {userUid}";

    public static string Deleted(string title, string userUid) => $"Course '{title}' was deleted by {userUid}";

    public static string Published(string title, string userUid) => $"Course '{title}' was published by {userUid}";

    public static string Activated(string title, string userUid) => $"Course '{title}' was activated by {userUid}";

    public static string Deactivated(string title, string userUid) => $"Course '{title}' was deactivated by {userUid}";

    public static string AddResource(string title, string userUid, int documentId) => $"Resource with ID: '{documentId}' was added to course '{title}' by {userUid}";

    public static string RemoveResource(string title, string userUid, int documentId) => $"Resource with ID: '{documentId}' was removed from course '{title}' by {userUid}";

}