using Payroll.Model;

public class LeaveAllocation
{
    public Guid Id { get; set; }
    public int NumberOfDays { get; set; }
    public DateTime DateCreated { get; set; }
    public int Period { get; set; }
    public Employee Employee { get; set; }
    public string EmployeeId { get; set; }
    
     public Guid LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; }    
   
}