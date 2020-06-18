using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWeb.Dto
{
    public class ResultsRow
    {
        public User User { get; set; }
        public List<Team> Teams { get; set; }
        public int BestBetId { get; set; }
        public int Rank { get; set; }
    }
}
