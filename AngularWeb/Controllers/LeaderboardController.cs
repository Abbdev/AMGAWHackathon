using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.Dto;
using AngularWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Leaderboard")]
    public class LeaderboardController : Controller
    {
        [HttpGet("[action]/{year}/{week}")]
        public IEnumerable<LeaderboardRow> GetLeaderboardForWeek(int week, int year)
        {
            return LeaderboardService.GetLeaderboardForWeek(week, year).OrderBy(x => x.Rank);
        }

        [HttpGet("[action]/{year}/{week}")]
        public ResultsResponse GetResultsForWeek(int week, int year)
        {
            return GameService.GetResultsForWeek(week, year);
        }
    }
}