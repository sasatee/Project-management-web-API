using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Payroll.Model;

namespace AuthenticationAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Payrolls> Payrolls { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Attendance>()
                .Property(a => a.OvertimeHours)
                .HasPrecision(18, 2);

            
            modelBuilder.Entity<JobTitle>()
                .Property(j => j.BaseSalary)
                .HasPrecision(18, 2);

            
            modelBuilder.Entity<Payrolls>()
                .Property(p => p.BasicSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payrolls>()
                .Property(p => p.Allowances)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payrolls>()
                .Property(p => p.Deductions)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payrolls>()
                .Property(p => p.NetPay)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LeaveAllocation>(entity =>
            {
                entity.HasOne(la => la.AppUser)
                    .WithMany(au => au.LeaveAllocations)
                    .HasForeignKey(la => la.AppUserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(la => la.Employee)
                    .WithMany()
                    .HasForeignKey(la => la.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(la => la.LeaveType)
                    .WithMany()
                    .HasForeignKey(la => la.LeaveTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
