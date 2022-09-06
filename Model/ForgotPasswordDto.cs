using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        public string ResetToken { get; set; } = string.Empty;
    }
}
