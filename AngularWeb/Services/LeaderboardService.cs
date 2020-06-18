using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;

namespace AngularWeb.Services
{
    public static class LeaderboardService
    {
        public static List<LeaderboardRow> GetLeaderboardForWeek(int week, int year)
        {
            var result = new List<LeaderboardRow>();

            var users = UserRepo.GetUsers();

            foreach (var user in users)
            {
                if (user.Picks != null)
                {
                    var currentWeek = user.Picks.Where(x => x.WeekNum == week && x.Year == year).FirstOrDefault();
                    if (currentWeek != null)
                    {
                        var previousWeek = new Pick();
                        if (week == 1)
                        {
                            previousWeek = currentWeek;
                        }
                        else
                        {
                            previousWeek = user.Picks.Where(x => x.WeekNum == week - 1 && x.Year == year).FirstOrDefault();
                        }

                        int totalCorrect = user.Picks.Where(x => x.WeekNum <= week && x.Year == year).Sum(x => x.Correct);
                        int totalIncorrect = user.Picks.Where(x => x.WeekNum <= week && x.Year == year).Sum(x => x.Total - x.Correct);
                        int total = user.Picks.Where(x => x.WeekNum <= week && x.Year == year).Sum(x => x.Total);
                        double totalPoint = user.Picks.Where(x => x.WeekNum <= week && x.Year == year).Sum(x => x.Points);
                        string lastWeek = previousWeek.Correct.ToString() + " - " + (previousWeek.Total - previousWeek.Correct).ToString() + " (" + previousWeek.Points.ToString() + ")";
                        double percent = (double)totalCorrect / (double)(total);

                        result.Add(new LeaderboardRow()
                        {
                            Rank = currentWeek.Rank,
                            Previous = previousWeek.Rank,
                            Name = user.Name,
                            Image = user.Image,
                            Email = user.Email,
                            Correct = totalCorrect,
                            Incorrect = totalIncorrect,
                            BestBet = currentWeek.BestBet.ToString(),
                            Percent = Double.IsInfinity(percent) ? 0 : percent,
                            LastWeek = lastWeek,
                            Points = totalPoint
                        });
                    }
                }
            }

            return result;
        }
    }
}
