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
    public static class GameRepo
    {
        public static List<Game> GetGamesForWeek (int week, int year)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");
            var collection = database.GetCollection<GameWeeks>("Games");

            var filter = Builders<GameWeeks>.Filter.Eq(x => x.WeekNum, week);
            filter = filter & Builders<GameWeeks>.Filter.Eq(x => x.Year, year);

            var gameweek = collection.Find(filter).FirstOrDefault();

            if (gameweek == null)
            {
                return new List<Game>();
            }
            else
            {
                return gameweek.Games;
            }
        }

        public static bool IsWeekLocked (int week, int year)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<GameWeeks>("Games");

            var filter = Builders<GameWeeks>.Filter.Eq(x => x.WeekNum, week);
            filter = filter & Builders<GameWeeks>.Filter.Eq(x => x.Year, year);

            var gameweek = collection.Find(filter).FirstOrDefault();

            if (gameweek == null)
            {
                return false;
            }

            return gameweek.Locked;
        }

        public static void SaveGamesForWeek (GameWeeks gameWeek)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<GameWeeks>("Games");

            var filter = Builders<GameWeeks>.Filter.Eq(x => x.WeekNum, gameWeek.WeekNum);
            filter = filter & Builders<GameWeeks>.Filter.Eq(x => x.Year, gameWeek.Year);

            collection.ReplaceOne(filter, gameWeek, new UpdateOptions { IsUpsert = true });
        }
    }
}
