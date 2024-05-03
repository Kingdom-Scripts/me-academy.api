using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App
{
    public class CourseVideo
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        [MaxLength(50)] public string UploadToken { get; set; } = null!;
        [MaxLength(255)] public string? VideoId { get; set; }
        public int VideoDuration { get; set; }

        [MaxLength(255)] public string? PreviewVideoId { get; set; }
        [MaxLength(255)] public string? PreviewThumbnailUrl { get; set; }
        public bool IsUploaded { get; set; } = false;

        public Course? Course { get; set; }
    }
}