namespace AuthenticationAPI.Controllers
{
     
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
