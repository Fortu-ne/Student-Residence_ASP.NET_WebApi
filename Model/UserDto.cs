using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model
{
    public class UserDto
    {
        [EmailAddress,Required]
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password's dont match"),Required]
        public string ConfirmPassword { get; set; }

        //public string Roles { get; set; }
    }
}
