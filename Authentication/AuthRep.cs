using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{
    public class AuthRep<T> : IAuthentication<T> where T : User
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext db;

        public AuthRep(IConfiguration config, AppDbContext db)
        {
            _config = config;
            this.db = db;
        }

        public void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
           using (var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public string CreateToken(T user)
        {
            var model = db.Students.FirstOrDefault(r => r.Id == user.Id);
            

            List<Claim> claims = new List<Claim>
           {
               new Claim(ClaimTypes.Name, user.Email),
               new Claim(ClaimTypes.Role, user.Roles),
               new Claim("User",user.Roles),
               new Claim("Identfier", user.Id),
               

           };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computerHash.SequenceEqual(PasswordHash);
            };
        }

        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
