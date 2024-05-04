﻿using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Models.App;

public class MeAcademyContext : DbContext
{
    public MeAcademyContext()
    { }

    public MeAcademyContext(DbContextOptions<MeAcademyContext> options) : base(options) { }

    public required DbSet<Code> Codes { get; set; }
    public required DbSet<Course> Courses { get; set; }
    public required DbSet<CourseAuditLog> CourseAuditLogs { get; set; }
    public required DbSet<CourseDocument> CourseDocuments { get; set; }
    public required DbSet<CourseLink> CourseLinks { get; set; }
    public required DbSet<CoursePrice> CoursePrices { get; set; }
    public required DbSet<CourseVideo> CourseVideos { get; set; }
    public required DbSet<CourseViewCount> CourseViewCounts { get; set; }
    public required DbSet<Document> Documents { get; set; }
    public required DbSet<Duration> Durations { get; set; }
    public required DbSet<QaOption> QaOptions { get; set; }
    public required DbSet<QaResponse> QaResponses { get; set; }
    public required DbSet<QuestionAndAnswer> QuestionAndAnswers  { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<UserCourse> UserCourses { get; set; }
    public required DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(t => new { t.RoleId, t.UserId });
        });

        builder.Entity<Duration>()
            .ToTable(p => p.HasCheckConstraint("CK_DurationType_Type", "[Type] IN ('Days', 'Weeks', 'Months', 'Years')"));

        builder.Entity<Course>()
            .HasIndex(c => c.Uid);

        builder.Entity<Document>()
            .HasOne(d => d.CreatedBy)
            .WithMany()
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<CourseDocument>()
            .HasOne(cd => cd.CreatedBy)
            .WithMany()
            .HasForeignKey(cd => cd.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<CourseAuditLog>()
            .HasOne(cal => cal.Course)
            .WithMany(c => c.CourseAuditLogs)
            .HasForeignKey(cal => cal.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<CourseAuditLog>()
            .HasOne(cal => cal.CreatedBy)
            .WithMany()
            .HasForeignKey(cal => cal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<QuestionAndAnswer>()
            .HasOne(cal => cal.CreatedBy)
            .WithMany()
            .HasForeignKey(cal => cal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<QaResponse>()
            .HasOne(cal => cal.CreatedBy)
            .WithMany()
            .HasForeignKey(cal => cal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}