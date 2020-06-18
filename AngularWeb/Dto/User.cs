using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularWeb.Dto
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool SendEmail { get; set; }
        public string Image { get; set; }
        public List<Pick> Picks { get; set; }
        public bool IsAdmin { get; set; }
        public string Salt { get; set; }
    }
}
