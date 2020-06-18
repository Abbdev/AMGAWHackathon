using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularWeb.Dto
{
    [BsonIgnoreExtraElements]
    public class Matchup
    {
        public int AwayTeamID { get; set; }
        public int HomeTeamID { get; set; }
        [BsonIgnore]
        public Team AwayTeam { get; set; }
        [BsonIgnore]
        public Team HomeTeam { get; set; }
        public int Week { get; set; }
        public int Season { get; set; }
        [BsonIgnore]
        public double Multiplier { get; set; }
        public string DateTime { get; set; }
        public string Day { get; set; }
        public double? PointSpread { get; set; }
    }
}
