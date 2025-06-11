namespace AuthenticationAPI.DTOs
{
    public class BulkAllowanceAssignDto
    {
        public Guid EmployeeId { get; set; }
        public List<AllowanceCreateDto> Allowances { get; set; }
    }
}
