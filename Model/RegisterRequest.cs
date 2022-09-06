using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model
{
    public class RegisterRequest
    {

        [Required]
        [EmailAddress]
        [Display(Name = "EmailAddress")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = " Confirm Password")]
        [Compare("Password", ErrorMessage = "The password is not the same")]
        public string ConfirmPassword { get; set; }
    
    }
}
