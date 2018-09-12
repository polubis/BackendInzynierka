using Inzynierka.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace Inzynierka.Repository.AppDbContext
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<Sound> Sounds { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Motive> Motives { get; set; }
        public DbSet<SharedMotives> SharedMotives { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasMany(g => g.Sounds).WithOne(e => e.User);
            modelBuilder.Entity<User>().HasMany(g => g.Quizes).WithOne(e => e.User);
            modelBuilder.Entity<User>().HasOne(g => g.Rate).WithOne(e => e.User);
            modelBuilder.Entity<Quiz>().HasMany(g => g.Questions).WithOne(e => e.Quiz);
            modelBuilder.Entity<User>().HasOne(g => g.UserSetting).WithOne(e => e.User);
            modelBuilder.Entity<User>().HasMany(g => g.Motives).WithOne(e => e.User);
            modelBuilder.Entity<UserSetting>().HasOne(g => g.Motive).WithOne(e => e.UserSetting);

            modelBuilder.Entity<SharedMotives>().HasOne(g => g.Motive).WithMany(e => e.SharedMotives).HasForeignKey(x => x.MotiveId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SharedMotives>().HasOne(g => g.User).WithMany(e => e.SharedMotives).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
