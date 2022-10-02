namespace ZillPillMobileApp.Domain.DTO.User
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public LoginResponseDto(string token)
        {
            Token = token;
        }
    }
}
