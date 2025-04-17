using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;

namespace AuthenticationAPI.Service
{
    public class PayrollService
    {
        private readonly IRepository<SalaryProgression> _salaryProgressionRepo;
        private readonly IRepository<CategoryGroup> _categoryGroupRepo;
        private readonly IRepository<SalaryStep> _salaryStepRepo;
        private readonly SeedSalaryForCategory _salaryStepSeeder;

        public PayrollService(
            IRepository<SalaryProgression> salaryRepo, 
            IRepository<CategoryGroup> categoryRepo,
            IRepository<SalaryStep> salaryStepRepo,
            SeedSalaryForCategory salaryStepSeeder)
        {
            _salaryProgressionRepo = salaryRepo;
            _categoryGroupRepo = categoryRepo;
            _salaryStepRepo = salaryStepRepo;
            _salaryStepSeeder = salaryStepSeeder;
        }

        public async Task<decimal> CalculateSalary(int yearOfService, Guid categoryGroupId)
        {
            var salaryprogression = await _salaryProgressionRepo.GetAll();
            var progressionForGroup = salaryprogression.Where(p => p.CategoryGroupId == categoryGroupId)
                .OrderBy(p => p.Year)
                .ToList();

            if(yearOfService <= progressionForGroup.Count)
            {
                var progression = progressionForGroup.FirstOrDefault(p => p.Year == yearOfService);
                return progression?.Salary ?? 0;
            }
            else
            {
                var maxSalary = progressionForGroup.LastOrDefault()?.Salary ?? 0;
                return maxSalary;
            }
        }

        public async Task<decimal> CalculateDynamicSalary(int yearsOfService, Guid categoryGroupId)
        {
            var allSteps = await _salaryStepRepo.GetAll(s => s.CategoryGroupId == categoryGroupId);
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
    }
}
