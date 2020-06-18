using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularWeb.Dto
{
    [BsonIgnoreExtraElements]
    public class Stats
    {
        public int Correct { get; set; }
        public double Percentage { get; set; }
        public int TopRank { get; set; }
        public int Incorrect { get; set; }
        public double Points { get; set; }
        public double TopPoints { get; set; }
    }
}
