using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{
    public class CreateCategoryGroupDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateCategoryGroupDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
    }
}
