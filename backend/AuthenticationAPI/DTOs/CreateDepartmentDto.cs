using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Controllers
{
    public partial class DepartmentController
    {
        public class CreateDepartmentDto
        {

            [Required(ErrorMessage = "DepartmentName is required")]
            public string DepartmentName { get; set; }
            public string HeadOfDepartment { get; set; }
        }
    }
}
