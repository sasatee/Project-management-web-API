using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Controllers
{
    public partial class DepartmentController
    {
        public class CreateDepartmentDto
        {
            
            [Required]
            public string DepartmentName { get; set; }
            public string HeadOfDepartment { get; set; }
        }
    }
}
