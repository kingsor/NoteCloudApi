using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NoteCloud.DataAccess;

namespace NoteCloudApi.Migrations
{
    [DbContext(typeof(NoteCloudContext))]
    [Migration("20161023050509_AddSalt")]
    partial class AddSalt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("NoteCloud.DataAccess.Follower", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FolloweeId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FolloweeId");

                    b.HasIndex("UserId");

                    b.ToTable("Followers");
                });

            modelBuilder.Entity("NoteCloud.DataAccess.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsComplete");

                    b.Property<bool>("IsPrivate");

                    b.Property<int>("NoteGroupId");

                    b.Property<int>("Priority");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("NoteGroupId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("NoteCloud.DataAccess.NoteGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("NoteGroups");
                });

            modelBuilder.Entity("NoteCloud.DataAccess.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Salt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NoteCloud.DataAccess.Follower", b =>
                {
                    b.HasOne("NoteCloud.DataAccess.User", "Followee")
                        .WithMany()
                        .HasForeignKey("FolloweeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NoteCloud.DataAccess.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoteCloud.DataAccess.Note", b =>
                {
                    b.HasOne("NoteCloud.DataAccess.NoteGroup", "NoteGroup")
                        .WithMany("Notes")
                        .HasForeignKey("NoteGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoteCloud.DataAccess.NoteGroup", b =>
                {
                    b.HasOne("NoteCloud.DataAccess.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
