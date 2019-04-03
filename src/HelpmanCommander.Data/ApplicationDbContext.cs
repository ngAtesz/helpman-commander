using System;
using HelpmanCommander.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpmanCommander.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Task>()
                .Property(t => t.IsDefaultTask)
                .HasDefaultValue(false);

            builder.Entity<Task>()
                .HasOne(t => t.PrerequisiteTask)
                .WithMany(t => t.DependentTasks)
                .HasForeignKey(t => t.PrerequisiteTaskId);

            builder.Entity<ExerciseTask>()
                .HasKey(et => new { et.ExerciseId, et.TaskId });
            builder.Entity<ExerciseTask>()
                .HasOne(et => et.Exercise)
                .WithMany(e => e.Tasks)
                .HasForeignKey(et => et.ExerciseId);
            builder.Entity<ExerciseTask>()
                .HasOne(et => et.Task)
                .WithMany(t => t.Exercises)
                .HasForeignKey(et => et.TaskId);

            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // TODO: create data seed for every entity
            // https://dzone.com/articles/quick-tip-seeding-large-data-in-entity-framework-c
        }
    }
}
