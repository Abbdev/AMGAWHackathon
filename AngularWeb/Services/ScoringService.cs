using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;

namespace AngularWeb.Services
{
    public static class ScoringService
    {
        public static void ScoreGamesForWeek(int week, int year)
        {
            var users = UserRepo.GetUsers();
            var gamesForWeek = GameRepo.GetGamesForWeek(week, year);

            var winnersPicked = new Dictionary<int, int>();
            var usersScores = new List<UserPoints>();

            // Fill up the winners picked dictionary
            foreach (var user in users)
            {
                Pick pick = null;
                if (user.Picks != null)
                {
                    pick = user.Picks.Where(x => x.Year == year && x.WeekNum == week).FirstOrDefault();
                }
                else
                {
                    user.Picks = new List<Pick>();
                }

                if (pick == null)
                {
                    // Select all the Home Teams
                    var winners = gamesForWeek.Select(x => x.HomeId).ToList();
                    pick = new Pick()
                    {
                        Correct = 0,
                        Points = 0,
                        Rank = 0,
                        BestBet = -1,
                        WeekNum = week,
                        Year = year,
                        Winners = winners
                    };

                    user.Picks.Add(pick);
                }

                foreach (int winner in pick.Winners)
                {
                    if (winnersPicked.ContainsKey(winner))
                    {
                        winnersPicked[winner]++;
                    }
                    else
                    {
                        winnersPicked.Add(winner, 1);
                    }
                }
            }

            // Score each user, if user does not have a pick then default them to the home teams
            foreach (var user in users)
            {
                var pick = user.Picks.Where(x => x.Year == year && x.WeekNum == week).FirstOrDefault();

                // Reset user to 0 points
                pick.Correct = 0;
                pick.Points = 0;
                pick.Total = 0;

                foreach (var game in gamesForWeek)
                {
                    if (game.Outcome == "Home")
                    {
                        if (pick.Winners.Contains(game.HomeId))
                        {
                            if (!winnersPicked.TryGetValue(game.AwayId, out int oppPoints))
                            {
                                oppPoints = 0;
                            }

                            double points = (users.Count + oppPoints) * game.Multiplier;

                            if (game.HomeId == pick.BestBet)
                            {
                                points = points * 2;
                            }

                            pick.Points = pick.Points + points;

                            pick.Correct++;
                        }
                        pick.Total++;
                    }
                    else if (game.Outcome == "Away")
                    {
                        if (pick.Winners.Contains(game.AwayId))
                        {
                            if (!winnersPicked.TryGetValue(game.HomeId, out int oppPoints))
                            {
                                oppPoints = 0;
                            }

                            double points = (users.Count + oppPoints) * game.Multiplier;

                            if (game.AwayId == pick.BestBet)
                            {
                                points = points * 2;
                            }

                            pick.Points = pick.Points + points;

                            pick.Correct++;
                        }
                        pick.Total++;
                    }
                }

                double totalPoint = user.Picks.Where(x => x.WeekNum <= week && x.Year == year).Sum(x => x.Points);

                usersScores.Add(new UserPoints()
                {
                    Email = user.Email,
                    Points = totalPoint
                });
            }

            // Calculate the Rankings    
            usersScores = usersScores.OrderByDescending(n => n.Points).Select((n, index) => new UserPoints() {
                Email = n.Email,
                Points = n.Points,
                Rank = index + 1
            }).ToList();

            // Update rank
            foreach (var user in users)
            {
                var pick = user.Picks.Where(x => x.Year == year && x.WeekNum == week).FirstOrDefault();
                pick.Rank = usersScores.Where(x => x.Email == user.Email).Select(x => x.Rank).FirstOrDefault();
                UserRepo.SaveUser(user);
            }
        }
    }

    public class UserPoints
    {
        public string Email { get; set; }
        public double Points { get; set; }
        public int Rank { get; set; }
    }
}
