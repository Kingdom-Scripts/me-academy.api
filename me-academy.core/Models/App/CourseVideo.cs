using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App
{
    public class CourseVideo
    {
        [Key]
        public int CourseId { get; set; }

        public string VideoId { get; set; } = null!;

        public string UploadToken { get; set; } = null!;

        public bool IsUploaded { get; set; } = false;

        public Course? Course { get; set; }
    }
}
