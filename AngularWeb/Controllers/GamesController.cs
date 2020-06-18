using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;
using AngularWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AngularWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Games")]
    public class GamesController : Controller
    {
        [HttpGet("[action]/{year}/{week}")]
        public IEnumerable<Game> GetGamesForWeek(int week, int year)
        {
            return GameService.GetGamesForWeek(week, year);
        }

        [HttpGet("[action]/{year}/{week}")]
        public bool IsWeekLocked(int week, int year)
        {
            return GameService.IsWeekLocked(week, year);
        }

        [HttpGet("[action]/{year}/{week}")]
        public IEnumerable<Matchup> GetMatchupsForWeek(int week, int year)
        {
            return ScheduleService.GetMatchupsForWeek(week, year);
        }

        [HttpPost("[action]")]
        public void SaveMatchupsForWeek([FromBody]Object gameWeeks)
        {
            var jsonString = gameWeeks.ToString();
            GameWeeks result = JsonConvert.DeserializeObject<GameWeeks>(jsonString);

            GameRepo.SaveGamesForWeek(result);
        }
        [HttpPost("[action]")]
        public void SendEmailForWeek([FromBody]Object gameWeeks)
        {
            var jsonString = gameWeeks.ToString();
            GameWeeks result = JsonConvert.DeserializeObject<GameWeeks>(jsonString);

            EmailService.SendEmailForWeek(result.WeekNum);
        }
        [HttpPost("[action]")]
        public void SaveGamePicksForWeek([FromBody]Object pick)
        {
            var jsonString = pick.ToString();
            UserPick result = JsonConvert.DeserializeObject<UserPick>(jsonString);
           
            User user = UserRepo.GetUser(result.Email);
           
            UserRepo.InsertPick(result.Pick, user);
        }
        [HttpPost("[action]")]
        public void ScoreWeek([FromBody]Object weekToScore)
        {
            var jsonString = weekToScore.ToString();
            WeekToScore result = JsonConvert.DeserializeObject<WeekToScore>(jsonString);

            ScoringService.ScoreGamesForWeek(result.Week, result.Year);
        }
        [HttpGet("[action]/{email}/{week}/{year}")]
        public Pick GetPickForWeek(string email, int week, int year)
        {
            return UserRepo.GetPickForWeek(email, week, year);

        }
        [HttpPost("[action]")]
        public void UpdateScheduleFromApi()
        {
            ScheduleService.UpdateScheduleFromApi();
        }
      
    }

    public class WeekToScore
    {
        public int Year { get; set; }
        public int Week { get; set; }
    }
    
    public class UserPick
    {
        public string Email { get; set; }
        public Pick Pick { get; set; }
    }
}
