using System;
using System.Diagnostics.Metrics;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{

    public class SchoolContext : IdentityDbContext<Person, IdentityRole<int>, int>
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        //public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        //public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        //public DbSet<CourseAssignment> CourseAssignments { get; set; }

        public DbSet<Person> People { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.OfficeAssignment)
                .WithOne(o => o.Instructor)
                .HasForeignKey<OfficeAssignment>(o => o.InstructorID);

            base.OnModelCreating(modelBuilder);

        }

public DbSet<ContosoUniversity.Models.Enrollment> Enrollment { get; set; } = default!;
    }
}