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
        

        public async Task SeedPayrollData()
        {
            if (!CategoryGroups.Any())
            {
                var categoryGroups = new List<CategoryGroup>
                {
                    new CategoryGroup { Id = Guid.NewGuid(), Name = "Junior Developer" },
                    new CategoryGroup { Id = Guid.NewGuid(), Name = "Senior Developer" },
                    new CategoryGroup { Id = Guid.NewGuid(), Name = "Team Lead" },
                    new CategoryGroup { Id = Guid.NewGuid(), Name = "Project Manager" }
                };

                await CategoryGroups.AddRangeAsync(categoryGroups);
                await SaveChangesAsync();

                // Seed salary progressions for each category
                var salaryProgressions = new List<SalaryProgression>();

                // Junior Developer progression
                for (int year = 1; year <= 5; year++)
                {
                    salaryProgressions.Add(new SalaryProgression
                    {
                        Id = Guid.NewGuid(),
                        Year = year,
                        Salary = 35000 + (year - 1) * 2500,
                        Increment = 2500,
                        CategoryGroupId = categoryGroups[0].Id
                    });
                }

                // Senior Developer progression
                for (int year = 1; year <= 5; year++)
                {
                    salaryProgressions.Add(new SalaryProgression
                    {
                        Id = Guid.NewGuid(),
                        Year = year,
                        Salary = 55000 + (year - 1) * 3500,
                        Increment = 3500,
                        CategoryGroupId = categoryGroups[1].Id
                    });
                }

                // Team Lead progression
                for (int year = 1; year <= 5; year++)
                {
                    salaryProgressions.Add(new SalaryProgression
                    {
                        Id = Guid.NewGuid(),
                        Year = year,
                        Salary = 75000 + (year - 1) * 5000,
                        Increment = 5000,
                        CategoryGroupId = categoryGroups[2].Id
                    });
                }

                // Project Manager progression
                for (int year = 1; year <= 5; year++)
                {
                    salaryProgressions.Add(new SalaryProgression
                    {
                        Id = Guid.NewGuid(),
                        Year = year,
                        Salary = 90000 + (year - 1) * 7500,
                        Increment = 7500,
                        CategoryGroupId = categoryGroups[3].Id
                    });
                }

                await SalaryProgressions.AddRangeAsync(salaryProgressions);
                await SaveChangesAsync();
            }
        }

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

            // Configure EmployeeTraining relationship
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

            // Configure relationships
            modelBuilder.Entity<SalaryProgression>()
                .HasOne(s => s.CategoryGroup)
                .WithMany(c => c.SalaryProgressions)
                .HasForeignKey(s => s.CategoryGroupId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
