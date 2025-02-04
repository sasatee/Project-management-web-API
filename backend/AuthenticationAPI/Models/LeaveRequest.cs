using Payroll.Model;

public class LeaveRequest{

    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime DateRequested { get; set; }
    public string RequestComments { get; set; }
    public bool? Approved { get; set; }
    public bool Cancelled { get; set; }
    public string ApprovedById { get; set; }

    
    public LeaveType LeaveType { get; set; }
    public int LeaveTypeId { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee> { };
    public int RequestingEmployeeId { get; set; }

}