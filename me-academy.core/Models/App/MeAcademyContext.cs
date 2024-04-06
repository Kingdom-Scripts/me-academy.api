using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Models.App;

public class MeAcademyContext : DbContext
{
    public MeAcademyContext()
    { }

    public MeAcademyContext(DbContextOptions<MeAcademyContext> options) : base(options) { }

    public required DbSet<Role> Roles { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<UserRole> UserRoles { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<Code> Codes { get; set; }
    public required DbSet<DurationType> DurationTypes { get; set; }
    public required DbSet<Course> Courses { get; set; }
    public required DbSet<CourseDocument> CourseDocuments { get; set; }
    public required DbSet<Document> Documents { get; set; }
    public required DbSet<CourseViewCount> CourseViewCounts { get; set; }
    public required DbSet<CourseAuditLog> CourseAuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(t => new { t.RoleId, t.UserId });
        });

        builder.Entity<DurationType>()
            .ToTable(p => p.HasCheckConstraint("CK_DurationType_Name", "[Name] IN ('Days', 'Weeks', 'Months', 'Years')"));

        builder.Entity<CourseDocument>()
            .HasKey(cd => new { cd.CourseId, cd.DocumentId });
    }
}