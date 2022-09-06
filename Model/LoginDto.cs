using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
