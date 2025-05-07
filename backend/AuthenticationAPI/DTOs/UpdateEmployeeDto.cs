namespace AuthenticationAPI.DTOs
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
       

        //job title model properties / request parameter
        public string? JobTitle { get; set; }  
    }
}
