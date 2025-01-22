namespace Payroll.Model
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string HeadOfDepartment { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
