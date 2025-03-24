using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;

namespace AuthenticationAPI.Service
{




    public class PayrollService
    {
        private readonly IRepository<SalaryProgression> _salaryProgressionRepo;
        private readonly IRepository<CategoryGroup> _categoryGroupRepo;

        public PayrollService(IRepository<SalaryProgression> salaryRepo , IRepository<CategoryGroup> categoryRepo)
        {
            _salaryProgressionRepo = salaryRepo;
            _categoryGroupRepo = categoryRepo;
            
        }


        public async Task<decimal> CalculateSalary(int yearOfService,Guid categoryGroupId)
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
    }
}
