using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);

            modelBuilder.Entity<AttendanceRecord>()
                .HasKey(a => a.AttendanceRecordId);

            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.AttendanceRecords)
                .HasForeignKey(a => a.EmployeeId);
        }
    }
}