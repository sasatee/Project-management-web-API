namespace AuthenticationAPI.DTOs
{
    public class CreateLeaveTypeDto
    {
       
        public string Name { get; set; }
        public int DefaultDays { get; set; }

    }

    public class ReponseLeaveType
    {

        public Guid Id { get; set; }

        public string Name { get; set; }
        public int DefaultDays { get; set; }

    }

    public class UpdateLeaveTypeDto
    {
        public string Name { get; set; }
        public int DefaultDays { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

}
