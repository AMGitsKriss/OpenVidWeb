using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.Users
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<ApplicationUser> User { get; set; }
        public virtual DbSet<IdentityUserClaim<string>> UserClaim { get; set; }
        public virtual DbSet<IdentityUserLogin<string>> UserLogin { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<IdentityUserRole<string>> UserRole { get; set; }
        public virtual DbSet<IdentityUserToken<string>> UserToken { get; set; }
        public virtual DbSet<IdentityRole> Role { get; set; }
        public virtual DbSet<IdentityRoleClaim<string>> RoleClaim { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionGroup> PermissionGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("User"); // Your custom IdentityUser class
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityRole>().ToTable("Role");

            builder.Entity<PermissionGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasData(
                    new { Id = 1, Name = "Account" },
                    new { Id = 2, Name = "Catalog" },
                    new { Id = 3, Name = "Tags" });
            });

            builder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.PermissionGroup)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.PermissionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permission_PermissionGroup");

                entity.HasData(
                    new { Id = 1, PermissionGroupId = 1, Name = "Management" },
                    new { Id = 2, PermissionGroupId = 2, Name = "Import" },
                    new { Id = 3, PermissionGroupId = 2, Name = "Delete" },
                    new { Id = 4, PermissionGroupId = 3, Name = "Management" });
            });

            builder.Entity<UserPermission>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PermissionId });

                entity.Property(e => e.UserId).HasMaxLength(100);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermission_Permission");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermission_User");
            });
        }
    }
}
