namespace AuthenticationAPI.DTOs
{
    public class AllowanceCreateDto
    {
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Remarks { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
