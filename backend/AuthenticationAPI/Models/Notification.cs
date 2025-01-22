﻿namespace Payroll.Model
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
