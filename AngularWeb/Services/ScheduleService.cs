using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AngularWeb.DataRepo;
using AngularWeb.Dto;

namespace AngularWeb.Services
{
    public static class ScheduleService
    {
        static HttpClient client = new HttpClient();
        public static List<Matchup> GetMatchupsForWeek(int week, int year)
        {
            var teams = TeamRepo.GetTeams();
            var matchups = ScheduleRepo.GetMatchupsForWeek(week, year);
            List<Matchup> matchups_copy = new List<Matchup>();
            foreach (var matchup in matchups)
            {
                matchup.HomeTeam = teams.GetValueOrDefault(matchup.HomeTeamID);
                matchup.AwayTeam = teams.GetValueOrDefault(matchup.AwayTeamID);
                matchup.Multiplier = 1;
                if (matchup.HomeTeam != null && matchup.AwayTeam != null)
                {
                    matchups_copy.Add(matchup);
                }
            }

            return matchups_copy.OrderByDescending(x => x.AwayTeam.School == "Michigan" || x.HomeTeam.School == "Michigan")
                .ThenByDescending(x => x.AwayTeam.School == "Michigan State" || x.HomeTeam.School == "Michigan State")
                .ThenByDescending(x => x.AwayTeam.School == "Ohio State" || x.HomeTeam.School == "Ohio State").ToList();
        }

        public static void UpdateScheduleFromApi()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://api.fantasydata.net/v3/cfb/scores/JSON/Games/2019")
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "49b2beac1b7c4f089cbd5baf61347e9d");

            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var stringResp = "";
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                stringResp = await response.Content.ReadAsStringAsync();
            }
            ScheduleRepo.SaveSchedule(stringResp);
        }
    }
}
