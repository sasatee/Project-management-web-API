using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;
using Payroll.Model;

namespace AuthenticationAPI.Service
{
    public class PayrollService
    {
        private readonly IRepository<SalaryProgression> _salaryProgressionRepo;
        private readonly IRepository<CategoryGroup> _categoryGroupRepo;
        private readonly IRepository<SalaryStep> _salaryStepRepo;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly SeedSalaryForCategory _salaryStepSeeder;

        public PayrollService(
            IRepository<SalaryProgression> salaryRepo, 
            IRepository<Employee> employeeRepo,
            IRepository<CategoryGroup> categoryRepo,
            IRepository<SalaryStep> salaryStepRepo,
            SeedSalaryForCategory salaryStepSeeder)
        {
            _employeeRepository = employeeRepo;
            _salaryProgressionRepo = salaryRepo;
            _categoryGroupRepo = categoryRepo;
            _salaryStepRepo = salaryStepRepo;
            _salaryStepSeeder = salaryStepSeeder;
        }


        public async Task<decimal> CalculateDynamicSalary(int yearsOfService, Guid categoryGroupId,Guid employeeId)
        {
            var allSteps = await _salaryStepRepo.GetAll(s => s.CategoryGroupId == categoryGroupId);
            var foundEmployee = await _employeeRepository.FindByIdAsync(employeeId);

            var orderedSteps = allSteps.OrderBy(s => s.FromAmount).ToList();

            if (!orderedSteps.Any())
                return 0;

            decimal currentSalary = orderedSteps.First().FromAmount;
            int remainingYears = yearsOfService;

            foreach (var step in orderedSteps)
            {
                int stepsAvailable = (int)Math.Floor((step.ToAmount - currentSalary) / step.Increment);
                
                int appliedSteps = Math.Min(remainingYears, stepsAvailable);
                
                currentSalary += appliedSteps * step.Increment;
                remainingYears -= appliedSteps;

                if (remainingYears <= 0)
                    break;

                currentSalary = step.ToAmount;

            }
            if (foundEmployee != null)
            {
                foundEmployee.CurrentSalary = currentSalary;
                _employeeRepository.Update(foundEmployee);
                await _employeeRepository.SaveChangesAsync();
            }


            return currentSalary;

        }
        
        public async Task SeedSalaryStepsForCategory(Guid categoryGroupId, string categoryName, List<SalaryStep> steps)
        {
            var categoryGroup = await _categoryGroupRepo.FindByIdAsync(categoryGroupId);
            
            if (categoryGroup == null)
            {
                categoryGroup = new CategoryGroup
                {
                    Id = categoryGroupId,
                    Name = categoryName
                };
                await _categoryGroupRepo.AddAsync(categoryGroup);
            }
            
            foreach (var step in steps)
            {
                step.CategoryGroupId = categoryGroupId;
                await _salaryStepRepo.AddAsync(step);
            }
            
            await _salaryStepRepo.SaveChangesAsync();
        }

        public async Task SeedUTM1SalarySteps()
        {
            await _salaryStepSeeder.SeedUTM1SalarySteps();
        }

        public async Task SeedUTM2SalarySteps()
        {
            await _salaryStepSeeder.SeedUTM2SalarySteps();
        }
        
        public async Task SeedUTM3SalarySteps()
        {
            await _salaryStepSeeder.SeedUTM3SalarySteps();
        }

      
    }
}
