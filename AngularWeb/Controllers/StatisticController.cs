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
    [Route("api/Statistic")]
    public class StatisticController : Controller
    {
        [HttpGet("[action]/{email}")]
        public Stats GetStatsForUser(string email)
        {
            return StatisticService.GetStatsforUser(email);
        }

        [HttpGet("[action]/{email}")]
        public IEnumerable<ProfileWeekly> GetProfileWeeks(string email)
        {
            return StatisticService.GetAllWeekStats(email);
        }
    }
}
