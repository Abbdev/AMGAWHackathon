using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWeb.Dto
{
    public class LeaderboardRow
    {
        public int Rank { get; set; }
        public int Previous { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public int Correct { get; set; }
        public int Incorrect { get; set; }
        public double Percent { get; set; }
        public string LastWeek { get; set; }
        public string BestBet { get; set; }
        public double Points { get; set; }
    }
}
