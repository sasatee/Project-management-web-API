namespace AuthenticationAPI.Controllers
{
    public partial class AllowanceController
    {
        ///TODO : find allowance with exployee
        ///payroll with allowance
        ///update 
        ///delete
        ///create
        ///
        public class GetAllowanceDto
        {
            public Guid Id { get; set; }
            public string? TypeName { get; set; }
            public string? Description { get; set; }

            public DateTime EffectiveDate { get; set; }
            public string? Remarks { get; set; }

            public DateTime ModifiedAt { get; set; }

           
        }
    }
}
