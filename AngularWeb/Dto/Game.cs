using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularWeb.Dto
{
    [BsonIgnoreExtraElements]
    public class Game
    {
        public string Away { get; set; }
        public int AwayId { get; set; }
        public string Home { get; set; }
        public int HomeId { get; set; }
        [BsonIgnore]
        public Team AwayTeam { get; set; }
        [BsonIgnore]
        public Team HomeTeam { get; set; }
        public double? Spread { get; set; }
        public double Multiplier { get; set; }
        public string Outcome { get; set; }
        [BsonIgnore]
        public string DateTime { get; set; }
        [BsonIgnore]
        public string Day { get; set; }
    }
}
