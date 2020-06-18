using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;
using System.Security.Cryptography;

namespace AngularWeb.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
    }
    public class LoginService
    {

        //public User Authenticate(string username, string password)
        //{
        //    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //        return null;

        //    var user = UserRepo.GetUsers().SingleOrDefault(x => x.Email == username);

        //    // check if username exists
        //    if (user == null)
        //        return null;

        //    // check if password is correct
        //    if (!VerifyPasswordHash(password, user.Password))
        //        return null;

        //    // authentication successful
        //    return user;
        //}
        //public static string hashPasswordGenerator(string password)
        //{
        //    System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
        //    StringBuilder hash = new StringBuilder();
        //    byte[] cry = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        //    return Convert.ToBase64String(cry);
        //}

        //public static bool VerifyPasswordHash(string password, string stringHash)
        //{
        //    System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
        //    StringBuilder hash = new StringBuilder();
        //    byte[] cry = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        //    if(stringHash == Convert.ToBase64String(cry))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }

        public static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

    }

    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
