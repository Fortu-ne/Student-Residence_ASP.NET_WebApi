using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model
{
    public class ResetPasswordDto
    {
        
        public string? Token { get; set; } = string.Empty;
       
        public string? Email { get; set; } = string.Empty;
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password's dont match")]
        public string ConfirmPassword { get; set; }
    }
}
