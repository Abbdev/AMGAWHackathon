using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AngularWeb.DataRepo;
using AngularWeb.Dto;
using AngularWeb.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularWeb.Controllers
{
    [Route("api/Registration")]
    public class RegController : Controller
    {
        [HttpPost("[action]")]
        public bool RegisterUser([FromBody]Object reg)
        {
            var jsonString = reg.ToString();
            RegParams result = JsonConvert.DeserializeObject<RegParams>(jsonString);
         
            User existUser = UserRepo.GetUser(result.Email);
            // User doesn't exist so create account
            if (existUser == null)
            {
                HashSalt hashSalt = LoginService.GenerateSaltedHash(10, result.Password);
                var user = new User
                {
                    Name = result.Name,
                    Email = result.Email,
                    Password = hashSalt.Hash,
                    SendEmail = result.SendEmail == "yes" ? true : false,
                    Picks = new List<Pick>(),
                    IsAdmin = false,
                    Image = result.Image,
                    Salt = hashSalt.Salt
                  
                };
                UserRepo.SaveUser(user);
                return true;
            }
            return false;
            
        }
    }

    public class RegParams
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Confirm_pw { get; set; }
        public string SendEmail { get; set; }
        public string Image { get; set; }
    }
}
