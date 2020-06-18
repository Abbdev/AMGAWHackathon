using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngularWeb.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AngularWeb.DataRepo
{
    public static class ScheduleRepo
    {
        public static List<Matchup> GetMatchupsForWeek(int week, int year)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<Matchup>("Schedule");

            var filter = Builders<Matchup>.Filter.Eq(x => x.Week, week);
            filter = filter & Builders<Matchup>.Filter.Eq(x => x.Season, year);

            var documents = collection.Find(filter).ToList();

            return documents;
        }

        public static void SaveSchedule(string jsonSchedule)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");
            database.DropCollection("Schedule");

            var collection = database.GetCollection<BsonDocument>("Schedule");
            var matchups = BsonSerializer.Deserialize<BsonDocument[]>(jsonSchedule);

            collection.InsertMany(matchups);

        }
    }
}
