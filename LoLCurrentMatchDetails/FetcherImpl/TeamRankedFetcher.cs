using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.CurrentGameEndpoint;
using RiotSharp;
using RiotSharp.TeamEndpoint;
using RiotSharp.CurrentGameEndpoint.Enums;
using RiotSharp.LeagueEndpoint;

namespace LoLCurrentMatchDetails
{
    class TeamRankedFetcher : GameFetcher
    {
        public void getQueueData(CurrentGame game, long summonerid)
        {
            List<Participant> participants = game.Participants;
            long teamId = participants.Find(x => x.SummonerId.Equals(summonerid)).TeamId;
            TeamInfo info;
            if(teamId == 100)
            {
                info = game.teamInfoOne;
            } else
            {
                info = game.teamInfoTwo;
            }

            if(info != null)
            {
                List<long> ids = new List<long>();
                ids.Add((long)summonerid);

                Dictionary<long,List<Team>> teams  = ApiFetcher.api.GetTeams(Region.euw, ids);
              
                
                string rankedteamid = null;
                info.teamId.TryGetValue("fullId", out rankedteamid);
                GameQueueType type = game.GameQueueType;
                List<Team> teamlist = teams.Values.First<List<Team>>();
                Team currentTeam = teamlist.Find(team => team.FullId.Equals(rankedteamid));
                
                TeamStatDetail details = null;
                if(type == GameQueueType.RankedTeam5x5)
                {
                    details = currentTeam.TeamStatDetails.Find(queue => queue.TeamStatType.Equals("RANKED_TEAM_5x5"));
                } else
                {
                    details = currentTeam.TeamStatDetails.Find(queue => queue.TeamStatType.Equals("RANKED_TEAM_3x3"));
                }

                string name = currentTeam.Name;

                try
                {
                        List<string> teamids = new List<string>();
                        teamids.Add(currentTeam.FullId);
                        Dictionary<string, List<League>> leaguedict = ApiFetcher.api.GetLeagues(Region.euw, teamids);
                        List<League> leagues = leaguedict.Values.First<List<League>>();
                        League league = leagues.Find(l => l.Queue.ToString().Equals(type.ToString()));
                        LeagueEntry entry = league.Entries[0];

                        string tier = league.Tier.ToString();
                        string division = entry.Division;
                        int points = entry.LeaguePoints;
                        int wins = entry.Wins;
                        int losses = entry.Losses;
                        FileWriter.WriteToFile(String.Format("{0}{1}{2} {3} ({4}LP) {5}/{6}", name, Environment.NewLine, tier, division, points, wins, losses));
                } catch(RiotSharpException e)
                {
                    FileWriter.WriteToFile(String.Format("{0}{1}{2} ({3}/{4})", name, Environment.NewLine, "Placements", details.Wins, details.Losses));
                }
                                                     
            }
        }
    }
}
