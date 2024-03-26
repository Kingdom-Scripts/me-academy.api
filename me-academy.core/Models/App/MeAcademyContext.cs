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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(t => new { t.RoleId, t.UserId });
        });
    }
}