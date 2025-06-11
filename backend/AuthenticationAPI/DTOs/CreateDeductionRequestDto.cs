using Microsoft.AspNetCore.Mvc;
using Payroll.Model;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOs
{


    public class CreateDeductionRequestDto
    {

        [Required(ErrorMessage = "TypeName is required")]
        public string? TypeName { get; set; }
        public decimal Amount { get; set; }
        public Guid EmployeeId { get; set; }
        public string? Remarks { get; set; }
   
    }


    public class CreateDeductionResponseDto
    {

        [Required(ErrorMessage = "TypeName is required")]
        public string? TypeName { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? Remarks { get; set; }

    }


    public class DeductionResponseDto
    {
        [Required(ErrorMessage = "TypeName is required")]
        public string? TypeName { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? Remarks { get; set; }
        public Guid EmployeeId { get; set; }
        public EmployeeBasicDto Employee { get; set; }
    }

    public class EmployeeBasicDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


}
