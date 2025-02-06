namespace Payroll.Model
{
    public class Department
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string HeadOfDepartment { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
