using AuthenticationAPI.Models;
using Payroll.Model;
using System.ComponentModel.DataAnnotations.Schema;

public class LeaveAllocation
{
    public Guid Id { get; set; }
    public int NumberOfDays { get; set; }
    public DateTime DateCreated { get; set; }
    public int Period { get; set; }

    [ForeignKey("EmployeeId")]
    public  Employee Employee { get; set; }
    public Guid? EmployeeId { get; set; }

    [ForeignKey("LeaveTypeId")]
    public  LeaveType LeaveType { get; set; }
    public Guid LeaveTypeId { get; set; }

    [ForeignKey("AppUserId")]
    public  AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
}