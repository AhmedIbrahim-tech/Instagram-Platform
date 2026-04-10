using Instagram.Areas.Identity.Data;
using Instagram.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Instagram.Data;

public class InstagramContext : IdentityDbContext<InstagramUser>
{
    public InstagramContext(DbContextOptions<InstagramContext> options)
        : base(options)
    {
    }

    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Seen> Seens => Set<Seen>();
    public DbSet<Story> Stories => Set<Story>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // أسماء جداول أوضح بدل بادئة AspNet*
        builder.Entity<InstagramUser>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
    }
}
