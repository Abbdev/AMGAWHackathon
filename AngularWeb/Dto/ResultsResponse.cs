using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularWeb.Dto
{
    public class ResultsResponse
    {
        public List<ResultsRow> Rows { get; set; }
        public List<Game> Games { get; set; }

    }
}
