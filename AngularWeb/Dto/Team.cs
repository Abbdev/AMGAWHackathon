using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularWeb.Dto
{
    [BsonIgnoreExtraElements]
    public class Team
    {
        public int TeamID { get; set; }
        public int? PlayoffRank { get; set; }
        public int? ApRank { get; set; }
        public string School { get; set; }
        public string TeamLogoUrl { get; set; }
        [BsonIgnore]
        public string Outcome { get; set; }
    }
}
