namespace Payroll.Model
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string PerformedBy { get; set; }
    }
}
