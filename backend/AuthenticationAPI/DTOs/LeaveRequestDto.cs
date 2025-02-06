public class LeaveRequestDto
{

public Guid Id {get; set;}

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
  public Guid LeaveTypeId {get; set;}
  public string RequestComments {get; set;}
}
