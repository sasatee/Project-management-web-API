using AuthenticationAPI.Models;
using AuthenticationAPI.Repository.IRepository;

namespace AuthenticationAPI.Service
{
    public class SeedSalaryForCategory
    {
        private readonly IRepository<SalaryStep> _salaryStepRepo;
        private readonly IRepository<CategoryGroup> _categoryGroupRepo;

        public SeedSalaryForCategory(
            IRepository<SalaryStep> salaryStepRepo,
            IRepository<CategoryGroup> categoryGroupRepo)
        {
            _salaryStepRepo = salaryStepRepo;
            _categoryGroupRepo = categoryGroupRepo;
        }

        public async Task SeedUTM1SalarySteps()
        {
           
            var categoryGroup = await GetOrCreateCategoryGroup("UTM 1");
            
          
            var existingSteps = await _salaryStepRepo.GetAll(s => s.CategoryGroupId == categoryGroup.Id);
            if (existingSteps.Any())
            {
                return;
            }
            
      
            var steps = new List<SalaryStep>
            {
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 10250, Increment = 175, ToAmount = 10775, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 10775, Increment = 200, ToAmount = 11775, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 11775, Increment = 205, ToAmount = 12595, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 12595, Increment = 230, ToAmount = 13975, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 13975, Increment = 250, ToAmount = 15225, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 15225, Increment = 260, ToAmount = 17825, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 17825, Increment = 275, ToAmount = 18925, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 18925, Increment = 300, ToAmount = 19525, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 19525, Increment = 325, ToAmount = 21150, CategoryGroupId = categoryGroup.Id }
            };


            foreach (var step in steps)
            {
                await _salaryStepRepo.AddAsync(step);
            }
            
            await _salaryStepRepo.SaveChangesAsync();
        }
        
        public async Task SeedUTM2SalarySteps()
        {
         
            var categoryGroup = await GetOrCreateCategoryGroup("UTM 2");
            
          
            var existingSteps = await _salaryStepRepo.GetAll(s => s.CategoryGroupId == categoryGroup.Id);
            if (existingSteps.Any())
            {
                return; 
            }
            
         
            var steps = new List<SalaryStep>
            {
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 13745, Increment = 230, ToAmount = 13975, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 13975, Increment = 250, ToAmount = 15225, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 15225, Increment = 260, ToAmount = 17825, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 17825, Increment = 275, ToAmount = 18925, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 18925, Increment = 300, ToAmount = 19525, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 19525, Increment = 325, ToAmount = 21475, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 21475, Increment = 375, ToAmount = 21850, CategoryGroupId = categoryGroup.Id }
            };

            foreach (var step in steps)
            {
                await _salaryStepRepo.AddAsync(step);
            }
            
            await _salaryStepRepo.SaveChangesAsync();
        }
        
        public async Task SeedUTM3SalarySteps()
        {
            // Create or get category group
            var categoryGroup = await GetOrCreateCategoryGroup("UTM 3");
            
            // Check if steps already exist
            var existingSteps = await _salaryStepRepo.GetAll(s => s.CategoryGroupId == categoryGroup.Id);
            if (existingSteps.Any())
            {
                return; // Already seeded
            }
            
            // Create salary steps for UTM 3 based on the provided increments and durations
            var steps = new List<SalaryStep>
            {
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 13745, Increment = 230, ToAmount = 13975, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 13975, Increment = 250, ToAmount = 15225, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 15225, Increment = 260, ToAmount = 17825, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 17825, Increment = 275, ToAmount = 18925, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 18925, Increment = 300, ToAmount = 19525, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 19525, Increment = 325, ToAmount = 21475, CategoryGroupId = categoryGroup.Id },
                new SalaryStep { Id = Guid.NewGuid(), FromAmount = 21475, Increment = 375, ToAmount = 22225, CategoryGroupId = categoryGroup.Id }
            };

            // Add salary steps
            foreach (var step in steps)
            {
                await _salaryStepRepo.AddAsync(step);
            }
            
            await _salaryStepRepo.SaveChangesAsync();
        }
        
        public async Task<CategoryGroup> GetOrCreateCategoryGroup(string name)
        {
            var categoryGroups = await _categoryGroupRepo.GetAll();
            var categoryGroup = categoryGroups.FirstOrDefault(cg => cg.Name == name);
            
            if (categoryGroup == null)
            {
                categoryGroup = new CategoryGroup
                {
                    Id = Guid.NewGuid(),
                    Name = name
                };
                
                await _categoryGroupRepo.AddAsync(categoryGroup);
                await _categoryGroupRepo.SaveChangesAsync();
            }
            
            return categoryGroup;
        }
    }
} 