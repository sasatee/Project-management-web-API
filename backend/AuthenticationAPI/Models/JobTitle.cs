namespace Payroll.Model
{
    public class JobTitle
    {
        public int JobTitleId { get; set; }
        public string Title { get; set; }
        public decimal BaseSalary { get; set; }
        public int Grade { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }

}
