using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App
{
    public class CourseVideo
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        [MaxLength(255)] public string? VideoId { get; set; }

        [MaxLength(50)] public string UploadToken { get; set; } = null!;

        public bool IsUploaded { get; set; } = false;

        public Course? Course { get; set; }
    }
}