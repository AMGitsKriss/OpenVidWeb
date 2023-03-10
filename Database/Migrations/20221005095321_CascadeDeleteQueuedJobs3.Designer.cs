// <auto-generated />
using System;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Migrations
{
    [DbContext(typeof(OpenVidContext))]
    [Migration("20221005095321_CascadeDeleteQueuedJobs3")]
    partial class CascadeDeleteQueuedJobs3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Database.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int>("PermissionGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PermissionGroupId");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Database.Models.PermissionGroup", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("PermissionGroup");
                });

            modelBuilder.Entity("Database.Models.Ratings", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("ID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Database.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("([NormalizedName] IS NOT NULL)");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Database.Models.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("RoleId")
                        .HasName("IX_RoleClaims_RoleId");

                    b.ToTable("RoleClaim");
                });

            modelBuilder.Entity("Database.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("ShortCode")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Type");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Database.Models.TagType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<bool>("ShowDescription")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("TagType");
                });

            modelBuilder.Entity("Database.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Database.Models.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .HasName("IX_UserClaims_UserId");

                    b.ToTable("UserClaim");
                });

            modelBuilder.Entity("Database.Models.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId")
                        .HasName("IX_UserLogins_UserId");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Database.Models.UserPermission", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("UserPermission");
                });

            modelBuilder.Entity("Database.Models.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("UserId", "RoleId")
                        .HasName("PK_UserRoles");

                    b.HasIndex("RoleId")
                        .HasName("IX_UserRoles_RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Database.Models.UserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("Database.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("Length")
                        .HasColumnType("time(0)");

                    b.Property<string>("MetaText")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int?>("RatingId")
                        .HasColumnName("RatingID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RatingId");

                    b.ToTable("Video");
                });

            modelBuilder.Entity("Database.Models.VideoEncodeQueue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Encoder")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.Property<string>("InputDirectory")
                        .IsRequired()
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<bool>("IsDone")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVertical")
                        .HasColumnType("bit");

                    b.Property<int>("MaxHeight")
                        .HasColumnType("int");

                    b.Property<string>("OutputDirectory")
                        .IsRequired()
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("PlaybackFormat")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasMaxLength(8)
                        .IsUnicode(false);

                    b.Property<double>("Quality")
                        .HasColumnType("float");

                    b.Property<string>("RenderSpeed")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.Property<string>("VideoFormat")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasMaxLength(8)
                        .IsUnicode(false);

                    b.Property<int>("VideoId")
                        .HasColumnName("VideoID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoEncodeQueue");
                });

            modelBuilder.Entity("Database.Models.VideoSegmentQueue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDone")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReady")
                        .HasColumnType("bit");

                    b.Property<int>("VideoId")
                        .HasColumnName("VideoID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoSegmentQueue");
                });

            modelBuilder.Entity("Database.Models.VideoSegmentQueueItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArgInputFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArgInputFolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .IsUnicode(true);

                    b.Property<string>("ArgLanguage")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("ArgStream")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<string>("ArgStreamFolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .IsUnicode(true);

                    b.Property<string>("ArgStreamId")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<int>("VideoId")
                        .HasColumnName("VideoID")
                        .HasColumnType("int");

                    b.Property<int>("VideoSegmentQueueId")
                        .HasColumnName("VideoSegmentQueueId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.HasIndex("VideoSegmentQueueId");

                    b.ToTable("VideoSegmentQueueItem");
                });

            modelBuilder.Entity("Database.Models.VideoSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasMaxLength(4)
                        .IsUnicode(false);

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Md5")
                        .IsRequired()
                        .HasColumnName("MD5")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<int>("VideoId")
                        .HasColumnName("VideoID")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Md5")
                        .IsUnique()
                        .HasName("IX_VideoSource_Unique");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoSource");
                });

            modelBuilder.Entity("Database.Models.VideoTag", b =>
                {
                    b.Property<int>("VideoId")
                        .HasColumnName("VideoID")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnName("TagID")
                        .HasColumnType("int");

                    b.HasKey("VideoId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("VideoTag");
                });

            modelBuilder.Entity("Database.Models.Permission", b =>
                {
                    b.HasOne("Database.Models.PermissionGroup", "PermissionGroup")
                        .WithMany("Permission")
                        .HasForeignKey("PermissionGroupId")
                        .HasConstraintName("FK_Permission_PermissionGroup")
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.RoleClaim", b =>
                {
                    b.HasOne("Database.Models.Role", "Role")
                        .WithMany("RoleClaim")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.Tag", b =>
                {
                    b.HasOne("Database.Models.TagType", "TypeNavigation")
                        .WithMany("Tag")
                        .HasForeignKey("Type")
                        .HasConstraintName("FK_Tag_TagType");
                });

            modelBuilder.Entity("Database.Models.UserClaim", b =>
                {
                    b.HasOne("Database.Models.User", "User")
                        .WithMany("UserClaim")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.UserLogin", b =>
                {
                    b.HasOne("Database.Models.User", "User")
                        .WithMany("UserLogin")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.UserPermission", b =>
                {
                    b.HasOne("Database.Models.Permission", "Permission")
                        .WithMany("UserPermission")
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("FK_UserPermission_Permission")
                        .IsRequired();

                    b.HasOne("Database.Models.User", "User")
                        .WithMany("UserPermission")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserPermission_User")
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.UserRole", b =>
                {
                    b.HasOne("Database.Models.Role", "Role")
                        .WithMany("UserRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.User", "User")
                        .WithMany("UserRole")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.UserToken", b =>
                {
                    b.HasOne("Database.Models.User", "User")
                        .WithMany("UserToken")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.Video", b =>
                {
                    b.HasOne("Database.Models.Ratings", "Rating")
                        .WithMany("Video")
                        .HasForeignKey("RatingId")
                        .HasConstraintName("FK__Video__RatingID__5CD6CB2B");
                });

            modelBuilder.Entity("Database.Models.VideoEncodeQueue", b =>
                {
                    b.HasOne("Database.Models.Video", "Video")
                        .WithMany("VideoEncodeQueue")
                        .HasForeignKey("VideoId")
                        .HasConstraintName("FK_VideoEncodeQueue_Video")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.VideoSegmentQueue", b =>
                {
                    b.HasOne("Database.Models.Video", "Video")
                        .WithMany("VideoSegmentQueue")
                        .HasForeignKey("VideoId")
                        .HasConstraintName("FK_VideoSegmentQueue_Video")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.VideoSegmentQueueItem", b =>
                {
                    b.HasOne("Database.Models.Video", "Video")
                        .WithMany("VideoSegmentQueueItem")
                        .HasForeignKey("VideoId")
                        .HasConstraintName("FK_VideoSegmentQueueItem_Video")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.VideoSegmentQueue", "VideoSegmentQueue")
                        .WithMany("VideoSegmentQueueItem")
                        .HasForeignKey("VideoSegmentQueueId")
                        .HasConstraintName("FK_VideoSegmentQueueItem_VideoSegmentQueue")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.VideoSource", b =>
                {
                    b.HasOne("Database.Models.Video", "Video")
                        .WithMany("VideoSource")
                        .HasForeignKey("VideoId")
                        .HasConstraintName("FK_VideoSource_Video")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Models.VideoTag", b =>
                {
                    b.HasOne("Database.Models.Tag", "Tag")
                        .WithMany("VideoTag")
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_VideoTag_Tag")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.Video", "Video")
                        .WithMany("VideoTag")
                        .HasForeignKey("VideoId")
                        .HasConstraintName("FK_VideoTag_VideoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
