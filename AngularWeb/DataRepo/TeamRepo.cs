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
    public static class TeamRepo
    {
        public static Dictionary<int, Team> GetTeams()
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<BsonDocument>("Teams");

            var documents = collection.Find(new BsonDocument()).ToList();

            var teams = new Dictionary<int, Team>();

            foreach (var document in documents)
            {
                var team = BsonSerializer.Deserialize<Team>(document);
                teams.Add(team.TeamID, team);
            }

            return teams;

        }

        public static Team GetTeamFromId(int teamId)
        {
            var client = new MongoClient("mongodb://Abb234:Actiondude2345!@ds137631.mlab.com:37631/fantasycollege");

            var database = client.GetDatabase("fantasycollege");

            var collection = database.GetCollection<Team>("Teams");

            var filter = Builders<Team>.Filter.Eq(x => x.TeamID, teamId);

            return collection.Find(filter).FirstOrDefault();
        }
    }
}
