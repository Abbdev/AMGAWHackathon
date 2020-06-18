using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularWeb.Dto
{
    [BsonIgnoreExtraElements]
    public class GameWeeks
    {
        public int Year { get; set; }
        public int WeekNum { get; set; }
        public List<Game> Games { get; set; }
        public bool Locked { get; set; }
    }
}
