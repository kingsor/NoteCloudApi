using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace NoteCloud.DataAccess
{
    public class NoteCloudContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteGroup> NoteGroups { get; set; }
        public DbSet<Follower> Followers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./notecloud.db");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }

    public class Follower 
    {
        public int Id { get; set; }

        //Person following
        public int UserId { get; set; }
        public User User { get; set; }

        //Person being followed
        public int FolloweeId { get; set; }
        public User Followee { get; set; }
    }

    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Priority { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPrivate { get; set; }

        public int NoteGroupId { get; set; }
        public NoteGroup NoteGroup { get; set; }
    }

    public class NoteGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public List<Note> Notes { get; set; }
    }
}