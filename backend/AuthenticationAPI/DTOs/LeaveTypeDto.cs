namespace AuthenticationAPI.DTOs
{
    public class LeaveTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DefaultDays { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

    public class CreateLeaveTypeRequest
    {
        public LeaveTypeDto LeaveTypeDto { get; set; }
        public LeaveType LeaveType { get; set; }
    }

}
