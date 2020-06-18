using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWeb.Dto
{
    public class Pick
    {
        public int Year { get; set; }
        public int WeekNum { get; set; }
        public int BestBet { get; set; }
        public double Points { get; set; }
        public int Correct { get; set; }
        public int Total { get; set; }
        public int Rank { get; set; }
        public List<int> Winners { get; set; }
    }
}
