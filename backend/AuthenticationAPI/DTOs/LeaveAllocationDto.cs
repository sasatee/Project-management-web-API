using System.ComponentModel.DataAnnotations;

public class LeaveAllocationDto
{
    public int Id { get; set; }
    public int NumberOfDays { get; set; }
    public int Period { get; set; }
    [Required]
    public string EmployeeId { get; set; }
    [Required]
    public int LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; }
}

public class CreateLeaveAllocationDto
{
    public int NumberOfDays { get; set; }
    public int Period { get; set; }
    [Required]
    public string EmployeeId { get; set; }
    [Required]
    public Guid LeaveTypeId { get; set; }

}