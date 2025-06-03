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
        
        public DbSet<SalaryProgression> SalaryProgressions { get; set; }
        public DbSet<CategoryGroup> CategoryGroups { get; set; }
        public DbSet<SalaryStep> SalarySteps { get; set; }

        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<Deduction> Deductions { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix decimal precision issues
            modelBuilder.Entity<SalaryProgression>()
                .Property(s => s.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalaryProgression>()
                .Property(s => s.Increment)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalaryStep>()
                .Property(s => s.FromAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalaryStep>()
                .Property(s => s.Increment)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalaryStep>()
                .Property(s => s.ToAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Employee>()
                .Property(e => e.CurrentSalary)
                .HasPrecision(18, 2);

            // Fix relationship issues
            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasOne(l => l.AppUser)
                    .WithMany(a => a.LeaveRequests)
                    .HasForeignKey(l => l.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(l => l.Employees)
                    .WithOne()
                    .HasForeignKey("LeaveRequestId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

          
            modelBuilder.Entity<EmployeeTraining>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(et => et.Employee)
                    .WithMany()
                    .HasForeignKey(et => et.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(et => et.Training)
                    .WithMany(t => t.EmployeeTrainings)
                    .HasForeignKey(et => et.TrainingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.CategoryGroup)
                .WithMany(cg => cg.Employees)
                .HasForeignKey(e => e.CategoryGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision
            modelBuilder.Entity<Attendance>()
                .Property(a => a.OvertimeHours)
                .HasPrecision(18, 2);

            modelBuilder.Entity<JobTitle>()
                .Property(j => j.BaseSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payrolls>(entity =>
            {
                entity.Property(p => p.BasicSalary).HasPrecision(18, 2);
                entity.Property(p => p.Allowances).HasPrecision(18, 2);
                entity.Property(p => p.Deductions).HasPrecision(18, 2);
                entity.Property(p => p.NetPay).HasPrecision(18, 2);
            });

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

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.AppUser)
                .WithOne()
                .HasForeignKey<Employee>(e => e.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);

     

            modelBuilder.Entity<SalaryStep>()
                .HasOne(s => s.CategoryGroup)
                .WithMany(c => c.SalarySteps)
                .HasForeignKey(s => s.CategoryGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
