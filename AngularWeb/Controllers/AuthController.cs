using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AngularWeb.Services;

namespace AngularWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        [HttpPost("[action]")]
        public LoginReturn LoginUser([FromBody]Object login)
        {
            var jsonString = login.ToString();
            LoginParams result = JsonConvert.DeserializeObject<LoginParams>(jsonString);

            var users = UserRepo.GetUsers();
            var user = users.Find(x => x.Email.ToLower() == result.Email.ToLower());


            if (user != null && LoginService.VerifyPassword(result.Password, user.Password,user.Salt))
            {
                
                return new LoginReturn()
                {
                    IsAdmin = user.IsAdmin,
                    LoggedIn = true
                };
            }

            return new LoginReturn()
            {
                IsAdmin = false,
                LoggedIn = false
            };
        }
        [HttpPost("[action]")]
        public bool ForgotPasswordEmail([FromBody]Object emailObject)
        {
            
            var jsonString = emailObject.ToString();
            EmailAddress result = JsonConvert.DeserializeObject<EmailAddress>(jsonString);
            User user = UserRepo.GetUser(result.Email);
            if(user != null)
            {
                EmailService.SendForgotPasswordEmail(result.Email);
                return true;
            }
            return false;
            
        }
        [HttpPost("[action]")]
        public void ResetPassword([FromBody]Object password)
        {
            var jsonString = password.ToString();
            PasswordString result = JsonConvert.DeserializeObject<PasswordString>(jsonString);
            HashSalt hashed = LoginService.GenerateSaltedHash(10,result.Password);
            string email = EncryptService.Decrypt(result.Encrypt, "astrophile");
            UserRepo.UpdatePassword(hashed.Hash, email);
            UserRepo.UpdateSalt(hashed.Salt, email);

        }
        [HttpGet("[action]/{email}")]
        public User GetUser(string email)
        {
            return UserRepo.GetUser(email);
        }

        [HttpGet("[action]")]
        public List<User> GetAllUsers()
        {
            return UserRepo.GetUsers();
        }

    }

    public class LoginParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginReturn
    {
        public bool IsAdmin { get; set; }
        public bool LoggedIn { get; set; }
    }

    public class EmailAddress
    {
        public string Email { get; set; }
    }
    public class PasswordString
    {
        public string Password { get; set; }
        public string Encrypt { get; set; }
    }
}