using System;
using Microsoft.EntityFrameworkCore;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Database.Models
{
    public partial class OpenVidContext : DbContext
    {
        public OpenVidContext()
        {
        }

        public OpenVidContext(DbContextOptions<OpenVidContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionGroup> PermissionGroup { get; set; }
        public virtual DbSet<Ratings> Ratings { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleClaim> RoleClaim { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagType> TagType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserClaim> UserClaim { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserToken> UserToken { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<VideoEncodeQueue> VideoEncodeQueue { get; set; }
        public virtual DbSet<VideoSegmentQueue> VideoSegmentQueue { get; set; }
        public virtual DbSet<VideoSegmentQueueItem> VideoSegmentQueueItem { get; set; }
        public virtual DbSet<VideoSource> VideoSource { get; set; }
        public virtual DbSet<VideoTag> VideoTag { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=ConnectionStrings:DefaultDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>(entity =>
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
            });

            modelBuilder.Entity<PermissionGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ratings>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_RoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaim)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Type).IsRequired(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShortCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Tag)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tag_TagType");
            });

            modelBuilder.Entity<TagType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClaim)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserLogins_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserPermission>(entity =>
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

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_UserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_UserRoles_RoleId");

                entity.Property(e => e.UserId).HasMaxLength(100);

                entity.Property(e => e.RoleId).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToken)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsUnicode(false);

                entity.Property(e => e.Length).HasColumnType("time(0)");

                entity.Property(e => e.MetaText).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.Video)
                    .HasForeignKey(d => d.RatingId)
                    .HasConstraintName("FK__Video__RatingID__5CD6CB2B");
            });

            modelBuilder.Entity<VideoEncodeQueue>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Encoder)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.InputDirectory)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.OutputDirectory)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.PlaybackFormat)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.RenderSpeed)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.VideoFormat)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.VideoId).HasColumnName("VideoID");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.VideoEncodeQueue)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("FK_VideoEncodeQueue_Video");
            });

            modelBuilder.Entity<VideoSegmentQueue>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VideoId).HasColumnName("VideoID");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.VideoSegmentQueue)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_VideoSegmentQueue_Video");
            });

            modelBuilder.Entity<VideoSegmentQueueItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VideoId).HasColumnName("VideoID");

                entity.Property(e => e.ArgInputFolder)
                    .IsRequired()
                    .IsUnicode(true);

                entity.Property(e => e.ArgStreamFolder)
                    .IsRequired()
                    .IsUnicode(true);

                entity.Property(e => e.ArgStream)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ArgStreamId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ArgLanguage)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.VideoSegmentQueueId).HasColumnName("VideoSegmentQueueId");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.VideoSegmentQueueItem)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_VideoSegmentQueueItem_Video");

                entity.HasOne(d => d.VideoSegmentQueue)
                    .WithMany(p => p.VideoSegmentQueueItem)
                    .HasForeignKey(d => d.VideoSegmentQueueId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_VideoSegmentQueueItem_VideoSegmentQueue");
            });

            modelBuilder.Entity<VideoSource>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Md5)
                    .IsRequired()
                    .HasColumnName("MD5")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.VideoId).HasColumnName("VideoID");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.VideoSource)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("FK_VideoSource_Video");
            });

            modelBuilder.Entity<VideoTag>(entity =>
            {
                entity.HasKey(e => new { e.VideoId, e.TagId });

                entity.Property(e => e.VideoId).HasColumnName("VideoID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.VideoTag)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_VideoTag_Tag");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.VideoTag)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("FK_VideoTag_VideoID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
