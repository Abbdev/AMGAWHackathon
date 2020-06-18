using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWeb.Dto
{
    public class ProfileWeekly
    {
        public int Week { get; set; }
        public int Rank { get; set; }
        public int Correct { get; set; }
        public int Incorrect { get; set; }
        public double Percent { get; set; }
        public string BestBet { get; set; }
        public double Points { get; set; }
    }
}
