using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;



namespace AngularWeb.Services
{
    public static class StatisticService
    {
        public static Stats GetStatsforUser(string email)
        {
            User user = UserRepo.GetUser(email);
            var result = new Stats();
            if (user.Picks.Count > 0)
            {
                int totalCorrect = user.Picks.Sum(x => x.Correct);
                int total = user.Picks.Sum(x => x.Total);
                double percent = (double)totalCorrect / (double)(total);
                int rank = user.Picks.Min(x => x.Rank);
                int totalIncorrect = total - totalCorrect;
                double topPoint = user.Picks.Max(x => x.Points);
                double totalPoint = user.Picks.Sum(x => x.Points);
                result.Correct = totalCorrect;
                result.Percentage = Math.Round(Math.Round(percent, 2) * 100, 0);
                result.TopRank = rank;
                result.Incorrect = totalIncorrect;
                result.Points = totalPoint;
                result.TopPoints = topPoint;
            }
            else
            {
                result.Correct = 0;
                result.Percentage = 0.0;
                result.TopRank = 0;
                result.Incorrect = 0;
                result.Points = 0;
                result.TopPoints = 0;
            }
            return result;
        }
        public static List<ProfileWeekly> GetAllWeekStats(string email)
        {
            User user = UserRepo.GetUser(email);
            var result = new List<ProfileWeekly>();
            foreach (var pick in user.Picks)
            {
                int week = pick.WeekNum;
                int rank = pick.Rank;
                int correct = pick.Correct;
                int incorrect = pick.Total - correct;
                double percent = (double)correct / (double)pick.Total;
                string bestBet;
                if(pick.BestBet == 0 || pick.BestBet == -1)
                {
                    bestBet = "None selected";
                }
                else
                {
                    bestBet = TeamRepo.GetTeamFromId(pick.BestBet).School;
                }
               
                double points = pick.Points;

                result.Add(new ProfileWeekly()
                {
                    Week = week,
                    Rank = rank,
                    Correct = correct,
                    Incorrect = incorrect,
                    Percent = Math.Round(Math.Round(percent,2) * 100,0),
                    BestBet = bestBet,
                    Points = points

                });
            }
            return result;
        }
    }
}
