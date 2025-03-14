using System.ComponentModel.DataAnnotations;

public class LeaveRequestDto
{

public Guid EmployeeId {get; set;}

public DateTime StartDate {get; set;}
public DateTime EndDate {get; set;}

public string RequestComments {get; set;}
public bool? Approved {get; set;}
public bool Cancelled {get; set;}

public Guid LeaveTypeId {get; set;}
public string LeaveTypeName {get; set;}

}


public class CreateLeaveRequestDto
{
  public DateTime StartDate {get; set;}
  public DateTime EndDate {get; set;}

  [Required]
  public Guid LeaveTypeId {get; set;}

  [Required]
  public string RequestComments {get; set;}
}
