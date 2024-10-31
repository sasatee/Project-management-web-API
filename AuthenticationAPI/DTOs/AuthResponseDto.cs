namespace AuthenticationAPI.DTOs
{
    public class AuthResponseDto
    {
        public string? Token { get; set; } = string.Empty;

        public bool isSuccess { get; set; }
        public string? Message { get; set; }

        public List<string> Roles { get; set; }


    }
}
