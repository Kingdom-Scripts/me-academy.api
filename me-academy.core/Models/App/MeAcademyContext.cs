using me_academy.core.Constants;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Models.App;

public class MeAcademyContext : DbContext
{
    public MeAcademyContext()
    { }

    public MeAcademyContext(DbContextOptions<MeAcademyContext> options) : base(options) { }

    public required DbSet<AnnotatedAgreement> AnnotatedAgreements { get; set; }
    public required DbSet<Code> Codes { get; set; }
    public required DbSet<Course> Courses { get; set; }
    public required DbSet<CourseDocument> CourseDocuments { get; set; }
    public required DbSet<CourseLink> CourseLinks { get; set; }
    public required DbSet<CoursePrice> CoursePrices { get; set; }
    public required DbSet<CourseVideo> CourseVideos { get; set; }
    public required DbSet<CourseViewCount> CourseViewCounts { get; set; }
    public required DbSet<Coupon> Coupons { get; set; }
    public required DbSet<Document> Documents { get; set; }
    public required DbSet<Duration> Durations { get; set; }
    public required DbSet<InvitedUser> InvitedUsers { get; set; }
    public required DbSet<Login> Logins { get; set; }
    public required DbSet<CourseQuestionOption> CourseQuestionOptions { get; set; }
    public required DbSet<CourseQuestionResponse> CourseQuestionResponses { get; set; }
    public required DbSet<CourseQuestion> CourseQuestions { get; set; }
    public required DbSet<Order> Orders { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<Series> Series { get; set; }
    public required DbSet<ContentLog> ContentLogs { get; set; }
    //public required DbSet<SeriesAuditLog> SeriesAuditLogs { get; set; }
    public required DbSet<SeriesCourse> SeriesCourses { get; set; }
    public required DbSet<SeriesPreview> SeriesPreviews { get; set; }
    public required DbSet<SeriesPrice> SeriesPrices { get; set; }
    public required DbSet<SeriesQuestion> SeriesQuestions { get; set; }
    public required DbSet<SeriesQuestionOption> SeriesQuestionOptions { get; set; }
    public required DbSet<SeriesQuestionResponse> SeriesQuestionResponses { get; set; }
    public required DbSet<SmeHub> SmeHubs { get; set; }
    public required DbSet<SmeHubType> SmeHubTypes { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<UserCourse> UserCourses { get; set; }
    public required DbSet<UserRole> UserRoles { get; set; }
    public required DbSet<UserSeries> UserSeries { get; set; }
    public required DbSet<SeriesProgress> SeriesProgress { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        //builder.Entity<Course>().ToTable("Courses");
        //builder.Entity<Series>().ToTable("Series");
        //builder.Entity<SmeHub>().ToTable("SmeHubs");
        //builder.Entity<AnnotatedAgreement>().ToTable("AnnotatedAgreements");

        //// Configure ContentBase as a base type
        //builder.Entity<Course>().HasBaseType<ContentBase>();
        //builder.Entity<Series>().HasBaseType<ContentBase>();
        //builder.Entity<SmeHub>().HasBaseType<ContentBase>();
        //builder.Entity<AnnotatedAgreement>().HasBaseType<ContentBase>();

        builder.Entity<Course>()
            .HasIndex(c => c.Uid);

        builder.Entity<ContentBase>()
            .HasDiscriminator<string>("ContentType")
            .HasValue<Course>("Course")
            .HasValue<SmeHub>("SmeHub")
            .HasValue<Series>("Series")
            .HasValue<AnnotatedAgreement>("AnnotatedAgreement");

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(t => new { t.RoleId, t.UserId });
        });

        // Make both UserId and Domain unique in Login
        builder.Entity<Login>()
            .HasIndex(l => new { l.UserId, l.Domain })
            .IsUnique();

        builder.Entity<Duration>()
            .ToTable(p => p.HasCheckConstraint("CK_DurationType_Type", "[Type] IN ('Days', 'Weeks', 'Months', 'Years')"));

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

        // TODO: remove the commented code
        //builder.Entity<CourseAuditLog>()
        //    .HasOne(cal => cal.Course)
        //    .WithMany(c => c.AuditLogs)
        //    .HasForeignKey(cal => cal.CourseId)
        //    .OnDelete(DeleteBehavior.NoAction);

        //builder.Entity<CourseAuditLog>()
        //    .HasOne(cal => cal.CreatedBy)
        //    .WithMany()
        //    .HasForeignKey(cal => cal.CreatedById)
        //    .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<CourseQuestion>()
            .HasOne(cal => cal.CreatedBy)
            .WithMany()
            .HasForeignKey(cal => cal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<CourseQuestionResponse>()
            .HasOne(cal => cal.CreatedBy)
            .WithMany()
            .HasForeignKey(cal => cal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<CourseQuestionResponse>()
            .HasOne(cal => cal.Course)
            .WithMany()
            .HasForeignKey(cal => cal.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SeriesQuestion>()
            .HasOne(cal => cal.CreatedBy)
            .WithMany()
            .HasForeignKey(cal => cal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SeriesQuestion>()
            .HasOne(cal => cal.Course)
            .WithMany()
            .HasForeignKey(cal => cal.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SeriesQuestionResponse>()
            .HasOne(cal => cal.CreatedBy)
            .WithMany()
            .HasForeignKey(cal => cal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SeriesCourse>()
            .HasOne(sal => sal.Series)
            .WithMany()
            .HasForeignKey(sal => sal.SeriesId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SeriesCourse>()
            .HasOne(sal => sal.CreatedBy)
            .WithMany()
            .HasForeignKey(sal => sal.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SeriesCourse>()
            .HasOne(sc => sc.Series)
            .WithMany(s => s.Courses) // Assuming Series has a collection of SeriesCourse
            .HasForeignKey(sc => sc.SeriesId);

        builder.Entity<SeriesCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.SeriesCourses) // Assuming Course has a collection of SeriesCourse
            .HasForeignKey(sc => sc.CourseId);

        builder.Entity<SeriesPreview>()
            .HasOne(sal => sal.Series)
            .WithOne(sal => sal.Preview)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SmeHub>()
            .HasOne(sh => sh.Document)
            .WithMany(sh => sh.SmeHubs)
            .HasForeignKey(sh => sh.DocumentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<AnnotatedAgreement>()
            .HasOne(sh => sh.Document)
            .WithMany(sh => sh.AnnotatedAgreements)
            .HasForeignKey(sh => sh.DocumentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ContentLog>()
            .ToTable(p => p.HasCheckConstraint("CK_ContentLog_ItemType", $"[ContentType] IN {ContentTypes.DB_CONSTRAINT}"))
            .HasOne(cl => cl.CreatedBy)
            .WithMany()
            .HasForeignKey(cl => cl.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Order>()
            .ToTable(p => p.HasCheckConstraint("CK_Order_ItemType", $"[ItemType] IN {ContentTypes.DB_CONSTRAINT}"))
            .HasOne(o => o.Content)
            .WithMany()
            .HasForeignKey(o => o.ContentId);

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}