using AuthenticationAPI.Models;
using Payroll.Model;

public class LeaveRequest{

    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime DateRequested { get; set; }
    public string RequestComments { get; set; }
    public bool? Approved { get; set; }
    public bool Cancelled { get; set; }
    public string? ApprovedById { get; set; }

    
    public Guid LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; }
    
    public string? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    
    public ICollection<Employee> Employees { get; set; } = new List<Employee> { };
    public Guid RequestingEmployeeId { get; set; }

}