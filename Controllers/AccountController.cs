using Auth0.ManagementApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using WepApiWithToken.Authentication;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
   
        private readonly IAuthentication<ServiceManagement> _authRep;
        private readonly IAuthentication<Student> _authStudentRep;
        private readonly IAuthentication<Manager> _authManagerRep;
        private readonly IServiceManager _userRep;
        private readonly AppDbContext _dbContext;
        private readonly IStudent _studRep;
        private readonly IManager _manager;
        private readonly IConfiguration config;

        public AccountController(IAuthentication<ServiceManagement> authServiceRep, IAuthentication<Student> authStudRep,IAuthentication<Manager> authManager, IServiceManager userRep, AppDbContext dbContext, IStudent studRep, IManager manager, IConfiguration config)
        {
            _authStudentRep = authStudRep;
            _authManagerRep = authManager;
            _authRep = authServiceRep;
            _userRep = userRep;
            _dbContext = dbContext;
            _studRep = studRep;
            _manager = manager;
            this.config = config;
        }

        
        [HttpPost("Manager/Register")]
        public async Task<ActionResult<string>> Register(UserDto request)
        {
              if(_manager.UserExistsByEmail(request.Email))
                {
                    return BadRequest("The Use Already exists in the Database");
                }

                _authManagerRep.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Manager user = new Manager()
            {
                PasswordHash = passwordHash,
                UserName = request.UserName,
                Name = request.Name,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordSalt = passwordSalt,
                DateAppointed = DateTime.Now,
                VerficationToken = _authRep.CreateRandomToken(),
            };

                _manager.Create(user);

            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            var userToken = user.VerficationToken;


            var confirmationlink = "http://localhost:4200/Account/verify-email?token="+ userToken;



            // send email 
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Email Confirmation For " + user.Name;


            email.Body = new TextPart(TextFormat.Html) { Text = "Hi " + user.Name  +"<br>"+ ", We just need to verify your email address before you can access[customer portal]." +"<br>"+"Verify your email address :" + confirmationlink + "<br>" + " – The RAS team" };


            using var smtp = new SmtpClient(); // using mailkit
            smtp.Connect(config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("Email:Username").Value, config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);


            return Ok(user);
            
            
        }

        [HttpPost("ServiceManagement/Register")]
        public async Task<ActionResult<string>> ServiceRegister(UserDto request)
        {
            if (_userRep.UserExistByEmail(request.Email))
            {
                return BadRequest("The Use Already exists in the Database");
            }

            _authRep.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            ServiceManagement user = new ServiceManagement()
            {
                PasswordHash = passwordHash,
                UserName = request.UserName,
                Name = request.Name,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordSalt = passwordSalt,
                VerficationToken = _authRep.CreateRandomToken(),
            };

            _userRep.Create(user);

            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            var userToken = user.VerficationToken;


            var confirmationlink = "http://localhost:4200/Account/verify-email?token=" + userToken;



            // send email 
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Email Confirmation For " + user.Name;


            email.Body = new TextPart(TextFormat.Html) { Text = "Hi " + user.Name + "<br>" + ", We just need to verify your email address before you can access[customer portal]." + "<br>" + "Verify your email address :" + confirmationlink + "<br>" + " – The RAS team" };


            using var smtp = new SmtpClient(); // using mailkit
            smtp.Connect(config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("Email:Username").Value, config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok(user);


        }

        [HttpPost("Student/Register")]
        public async Task<ActionResult<string>> StudentRegister(UserDto request)
        {
            if (_studRep.StudentExist(request.Email))
            {
                return BadRequest("The Use Already exists in the Database");
            }

            _authStudentRep.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            Student user = new Student()
            {
            PasswordHash = passwordHash,
            UserName = request.UserName,
            Name = request.Name,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            PasswordSalt = passwordSalt,
            VerficationToken = _authRep.CreateRandomToken(),
        };
            _studRep.Create(user);

            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            var userToken = user.VerficationToken;
            var confirmationlink = "http://localhost:4200/Account/verify-email?token=" + userToken;

            // send email 
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Email Confirmation For " + user.Name;
            email.Body = new TextPart(TextFormat.Html) { Text = "Hi " + user.Name + "<br>" + ", We just need to verify your email address before you can access[customer portal]." + "<br>" + "Verify your email address :" + confirmationlink + "<br>" + " – The RAS team" };

            // stmp configuration
            using var smtp = new SmtpClient(); // using mailkit
            smtp.Connect(config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("Email:Username").Value, config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok(user);


        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> ManagerLogin(LoginDto request)
        {
            
            var currentUser = _manager.GetUserByEmail(request.Email);
            var serviceUser = _userRep.GetUserByEmail(request.Email);
            var studUser = _studRep.GetStudentByEmail(request.Email);

            var userRole="";

            var token = "";

            if (currentUser != null)
            {
                if (!_manager.UserExistsByEmail(request.Email))
                {
                    return BadRequest("User not found.");
                }

                if (!_authManagerRep.VerifyPasswordHash(request.Password, currentUser.PasswordHash, currentUser.PasswordSalt))
                {
                    return BadRequest("Wrong password.");
                }


                if (currentUser.VerifiedAt == null)
                {
                    return BadRequest("user not verified");
                }

                 token = _authManagerRep.CreateToken(currentUser);
                userRole = currentUser.Roles;
            }
            else if (serviceUser != null) {
                if (!_userRep.UserExistByEmail(request.Email))
                {
                    return BadRequest("User not found.");
                }



                if (!_authRep.VerifyPasswordHash(request.Password, serviceUser.PasswordHash, serviceUser.PasswordSalt))
                {
                    return BadRequest("Wrong password.");
                }


                if (serviceUser.VerifiedAt == null)
                {
                    return BadRequest("user not verified");
                }

                 token = _authRep.CreateToken(serviceUser);
                userRole = serviceUser.Roles;
            } 
            else {
                if (!_studRep.StudentExistByEmail(request.Email))
                {
                    return BadRequest("User not found.");
                }



                if (!_authStudentRep.VerifyPasswordHash(request.Password, studUser.PasswordHash, studUser.PasswordSalt))
                {
                    return BadRequest("Wrong password.");
                }


                if (studUser.VerifiedAt == null)
                {
                    return BadRequest("user not verified");
                }

                 token = _authStudentRep.CreateToken(studUser);
                userRole = studUser.Roles;
            }

           

            return Ok(token) ;

        }

        [HttpGet("ConfirmEmailLink")]
        public async Task<IActionResult> ConfirmEmailLink(string token)
        {
            var currentUser = _dbContext.Managers.FirstOrDefault(i => i.VerficationToken == token);
            var studUser = _dbContext.Students.FirstOrDefault(i => i.VerficationToken == token);

            var serviceUser = _dbContext.ServiceManagements.FirstOrDefault(i => i.VerficationToken == token);



            if (currentUser != null)
            {
                
                if (currentUser == null)
            {
                    return BadRequest("Invalid token.");
                }
                currentUser.VerifiedAt = DateTime.Now;
            }
            else if (serviceUser != null)
            {
                
                if (serviceUser == null) 
            {
                    return BadRequest("Invalid token.");
                }
                serviceUser.VerifiedAt = DateTime.Now;
            }
            else
            { 
                if (studUser == null)
                {
                    return BadRequest("Invalid token.");
                }
                studUser.VerifiedAt = DateTime.Now;
            }



                //currentUser.VerifiedAt = DateTime.Now;
                _dbContext.SaveChanges();
            

            return Ok("User verified :");

        }

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string userEmail)
        {
         
            var Manageruser = _manager.GetUserByEmail(userEmail);
            var ServiceUser = _userRep.GetUserByEmail(userEmail);
            var user = _studRep.GetStudentByEmail(userEmail);

            var token = "";
            var confirmationlink = "";
            var email = new MimeMessage();

            if (Manageruser != null)
            {
                if (Manageruser.Email == null)
                {
                    return BadRequest("User is not found");
                }

                Manageruser.PasswordRestToken = _authManagerRep.CreateRandomToken();
                Manageruser.TokenExpires = DateTime.Now.AddMinutes(3);

                token = Manageruser.PasswordRestToken;

                confirmationlink = "http://localhost:4200/Account/reset-password?token=" + token + "&email=" + Manageruser.Email;

                
                email.From.Add(MailboxAddress.Parse(config.GetSection("Email:Username").Value));
                email.To.Add(MailboxAddress.Parse(Manageruser.Email));
                email.Subject = "Reset Password " + Manageruser.Name;


                email.Body = new TextPart(TextFormat.Html) { Text = "Hi " + Manageruser.Name + "<br>" + ",Please click on the link below to reset your password" + "<br>" + "Reset Password :" + confirmationlink + "<br></br>" + "If this was not you, please don't respond to this email" + "<br>" + " – The RAS team" };

            }
            else if (ServiceUser != null)
            {
                if (ServiceUser.Email == null)
                {
                    return BadRequest("User is not found");
                }

                ServiceUser.PasswordRestToken = _authRep.CreateRandomToken();
                ServiceUser.TokenExpires = DateTime.Now.AddMinutes(3);

                token = ServiceUser.PasswordRestToken;

                confirmationlink = "http://localhost:4200/Account/reset-password?token=" + token + "&email=" + ServiceUser.Email;


                // send email 
                
                email.From.Add(MailboxAddress.Parse(config.GetSection("Email:Username").Value));
                email.To.Add(MailboxAddress.Parse(ServiceUser.Email));
                email.Subject = "Reset Password " + ServiceUser.Name;


                email.Body = new TextPart(TextFormat.Html) { Text = "Hi " + ServiceUser.Name + "<br>" + ",Please click on the link below to reset your password" + "<br>" + "Reset Password :" + confirmationlink + "<br></br>" + "If this was not you, please don't respond to this email" + "<br>" + " – The RAS team" };


            }
            else
            {
                if (user.Email == null)
                {
                    return BadRequest("User is not found");
                }

                user.PasswordRestToken = _authStudentRep.CreateRandomToken();
                user.TokenExpires = DateTime.Now.AddMinutes(3);

                token = user.PasswordRestToken;

                confirmationlink = "http://localhost:4200/Account/reset-password?token=" + token + "&email=" + user.Email;

                email.From.Add(MailboxAddress.Parse(config.GetSection("Email:Username").Value));
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = "Reset Password " + user.Name;


                email.Body = new TextPart(TextFormat.Html) { Text = "Hi " + user.Name + "<br>" + ", Please click on the link below to reset your password" + "<br>" + "Reset Password Link :" + confirmationlink + "<br></br>" + "If this was not you, please don't respond to this email" + "<br>" + " – The RAS team" };


            }



           

            using var smtp = new SmtpClient(); // using mailkit
            smtp.Connect(config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("Email:Username").Value, config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

            await _dbContext.SaveChangesAsync();




            return Ok("You may reset your password now ");

        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var currentUser = _dbContext.Managers.FirstOrDefault(u => u.PasswordRestToken == request.Token);
            var ServiceUser = _dbContext.ServiceManagements.FirstOrDefault(u => u.PasswordRestToken == request.Token);
            var studUser = _dbContext.Students.FirstOrDefault(u => u.PasswordRestToken == request.Token);

            if (currentUser != null)
            {
                if (currentUser == null || currentUser.RestTokenExpires < DateTime.Now)
                {
                    return BadRequest("Invalid Token");
                }

                _authManagerRep.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                currentUser.PasswordHash = passwordHash;
                currentUser.PasswordSalt = passwordSalt;
                currentUser.PasswordRestToken = null;
                currentUser.RestTokenExpires = null;
            }
            else if(ServiceUser != null)
            {
                if (ServiceUser == null || ServiceUser.RestTokenExpires < DateTime.Now)
                {
                    return BadRequest("Invalid Token");
                }

                _authRep.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                ServiceUser.PasswordHash = passwordHash;
                ServiceUser.PasswordSalt = passwordSalt;
                ServiceUser.PasswordRestToken = null;
                ServiceUser.RestTokenExpires = null;
            }
            else
            {
                if (studUser == null || studUser.RestTokenExpires < DateTime.Now)
                {
                    return BadRequest("Invalid Token");
                }

                _authStudentRep.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                studUser.PasswordHash = passwordHash;
                studUser.PasswordSalt = passwordSalt;
                studUser.PasswordRestToken = null;
                studUser.RestTokenExpires = null;
            }


            await _dbContext.SaveChangesAsync();


            return Ok("Password has been reseted");
        }

        //[HttpPost("Register")]
        //public async Task<ActionResult<string>> Register(UserDto request)
        //{

        //    if (_userRep.UseExists(request.Email))
        //    {
        //        return BadRequest("The Use Already exists in the Database");
        //    }

        //    _authRep.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        //    user.Email = request.Email;
        //    user.UserName = request.UserName;
        //    user.Roles = request.Roles;
        //    user.PasswordHash = passwordHash;
        //    user.PasswordSalt = passwordSalt;
        //    user.VerficationToken = _authRep.CreateRandomToken();

        //    _userRep.Create(user);
        //    return Ok(user);
        //}




        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(LoginDto request)
        //{
        //    var user = new ServiceManagement();   
        //    var currentUser = _userRep.GetUser(request.Email);

        //   if(request.Email != currentUser.Email)
        //    {
        //        return BadRequest("User not found.");
        //    }



        //    if (!_authRep.VerifyPasswordHash(request.Password, currentUser.PasswordHash, currentUser.PasswordSalt))
        //    {
        //        return BadRequest("Wrong password.");
        //    }


        //    if (currentUser.VerifiedAt == null)
        //    {
        //        return BadRequest("user not verified");
        //    }

        //    string token = _authRep.CreateToken(user);

        //    //var refreshToken = GenerateRefreshToken();
        //    //SetRefreshToken(refreshToken);

        //    return Ok(token);

        //}

        //[HttpPost("Student/Verify")]
        //public async Task<IActionResult> Verfiy(string token)
        //{
        //    var currentUser =  _dbContext.Students.FirstOrDefault(i => i.VerficationToken == token); 


        //    if (currentUser == null)
        //    {
        //        return BadRequest("Invalid token.");
        //    }

        //    currentUser.VerifiedAt = DateTime.Now;
        //    _dbContext.SaveChanges();

        //    return Ok("User verified :");

        //}


        //[HttpPost("SendEmail")]
        //public async Task<IActionResult> SendEmail(string userEmail)
        //{


        //    var user = _manager.GetUser(userEmail);

        //    if (user == null)
        //    {
        //        return BadRequest("Invalid token.");
        //    }

        //    var userToken = user.VerficationToken;


        //    //var confirmationlink = "https://localhost:7294/api/Account/ConfirmEmailLink?token=" + userToken;
        //    var confirmationlink = "http://localhost:4200/Account/verify-email?token=" + userToken;


        //    // send email 
        //    var email = new MimeMessage();
        //    email.From.Add(MailboxAddress.Parse(config.GetSection("Email:Username").Value));
        //    email.To.Add(MailboxAddress.Parse(user.Email));
        //    email.Subject = "Email Confirmation For " + user.UserName;


        //    email.Body = new TextPart(TextFormat.Html) { Text = "Hi " + user.UserName + ", \n We just need to verify your email address before you can access[customer portal].\n\nVerify your email address :" + confirmationlink + "\n – The RAS team" };


        //    using var smtp = new SmtpClient(); // using mailkit
        //    smtp.Connect(config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
        //    smtp.Authenticate(config.GetSection("Email:Username").Value, config.GetSection("Email:Password").Value);
        //    smtp.Send(email);
        //    smtp.Disconnect(true);



        //    return Ok("Email Sent");

        //}
    }
}
