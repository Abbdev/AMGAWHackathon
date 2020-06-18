using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;

namespace AngularWeb.Services
{
    public static class GameService
    {
        public static List<Game> GetGamesForWeek(int week, int year)
        {
            var games = GameRepo.GetGamesForWeek(week, year);
            var teams = TeamRepo.GetTeams();

            foreach (var game in games)
            {
                game.HomeTeam = teams.GetValueOrDefault(game.HomeId);
                game.AwayTeam = teams.GetValueOrDefault(game.AwayId);
            }

            return games;
        }

        public static bool IsWeekLocked(int week, int year)
        {
            return GameRepo.IsWeekLocked(week, year);
        }

        public static ResultsResponse GetResultsForWeek(int week, int year)
        {
            var result = new ResultsResponse();
            var resultRows = new List<ResultsRow>();

            var users = UserRepo.GetUsers();
            var games = GameRepo.GetGamesForWeek(week, year);
            var teams = TeamRepo.GetTeams();
            var matchups = ScheduleRepo.GetMatchupsForWeek(week, year);

            foreach (var game in games)
            {
                game.HomeTeam = teams.GetValueOrDefault(game.HomeId);
                game.AwayTeam = teams.GetValueOrDefault(game.AwayId);
                game.DateTime = matchups.Where(x => x.HomeTeamID == game.HomeId).Select(x => x.DateTime).FirstOrDefault();
                game.Day = matchups.Where(x => x.HomeTeamID == game.HomeId).Select(x => x.Day).FirstOrDefault();

                if (game.Outcome == "Home")
                {
                    game.HomeTeam.Outcome = "Win";
                    game.AwayTeam.Outcome = "Lose";
                }
                else if (game.Outcome == "Away")
                {
                    game.AwayTeam.Outcome = "Win";
                    game.HomeTeam.Outcome = "Lose";
                }
            }

            foreach (var user in users)
            {
                if (user.Picks != null)
                {
                    var pick = user.Picks.Where(x => x.WeekNum == week && x.Year == year).FirstOrDefault();
                    if (pick != null)
                    {
                        var prevPick = new Pick();
                        if (week == 1)
                        {
                            prevPick = pick;
                        }
                        else
                        {
                            prevPick = user.Picks.Where(x => x.WeekNum == week - 1 && x.Year == year).FirstOrDefault();
                        }

                        var userTeams = new List<Team>();

                        foreach (var game in pick.Winners)
                        {
                            userTeams.Add(teams[game]);
                        }

                        user.Password = "";
                        user.Picks = null;

                        resultRows.Add(new ResultsRow()
                        {
                            BestBetId = pick.BestBet,
                            Rank = prevPick.Rank,
                            Teams = userTeams,
                            User = user
                        });
                    }
                }
            }

            result.Rows = resultRows.OrderBy(x => x.Rank).ToList();

            result.Games = games;

            return result;
        }

       
    }
}
