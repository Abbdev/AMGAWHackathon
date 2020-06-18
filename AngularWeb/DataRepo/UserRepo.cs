using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AngularWeb.DataRepo
{
    public static class UserRepo
    {
        public static List<User> GetUsers()
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<BsonDocument>("Users");

            var documents = collection.Find(new BsonDocument()).ToList();

            var result = new List<User>();

            foreach (var document in documents)
            {   
                result.Add(BsonSerializer.Deserialize<User>(document));
            }

            return result;
        }
        public static User GetUser(string email)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<User>("Users");

            var filter = Builders<User>.Filter.Eq(x => x.Email, email);

            return collection.Find(filter).FirstOrDefault();

        }
        public static void SaveUser(User user)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<User>("Users");
            var filter = Builders<User>.Filter.Eq(x => x.Email, user.Email);

            collection.ReplaceOne(filter, user, new UpdateOptions { IsUpsert = true });
        }
        public static void InsertPick(Pick pick, User user)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<User>("Users");

            var filter = Builders<User>.Filter.Eq(x => x.Email, user.Email);

            var weekNum = pick.WeekNum;
            var year = pick.Year;
            var userPick = user.Picks.Where(p => p.Year == year && p.WeekNum == weekNum).FirstOrDefault();
            if (userPick != null)
            {
                user.Picks.Remove(userPick);
            }
            
            user.Picks.Add(pick);
            
           


            collection.ReplaceOne(filter, user, new UpdateOptions { IsUpsert = true });

        }

        public static Pick GetPickForWeek(string email, int week, int year)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<User>("Users");

            var filter = Builders<User>.Filter.Eq("Email", email);

            var result = collection.Find(filter).FirstOrDefault();

            var pick = result.Picks.Find(r => r.Year == year && r.WeekNum == week);

            return pick;

        }

        public static void UpdatePassword(string password, string email)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<User>("Users");

            var filter = Builders<User>.Filter.Eq(x => x.Email, email);

            collection.UpdateOne(x => x.Email == email,
                Builders<User>.Update.Set(x => x.Password, password));
        }

        public static void UpdateSalt(string salt, string email)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<User>("Users");

            var filter = Builders<User>.Filter.Eq(x => x.Email, email);

            collection.UpdateOne(x => x.Email == email,
                Builders<User>.Update.Set(x => x.Salt, salt));
        }
    }
}
