using System;
using System.Collections.Generic;
using System.Linq;
using RiotSharp.CurrentGameEndpoint;
using RiotSharp;
using RiotSharp.LeagueEndpoint;
using RiotSharp.LeagueEndpoint.Enums;

namespace LoLCurrentMatchDetails
{
    class SoloQueueFetcher : GameFetcher
    {
        public string getQueueData(CurrentGame game,long summonerid)
        {
            Participant summoner = null;
            game.Participants.ForEach(user => {
                long userid = user.SummonerId;
                if(userid == summonerid)
                {
                    summoner = user;
                }
            });
            List<long> summonerList = new List<long>();
            summonerList.Add(unchecked((int)summoner.SummonerId));
            try
            {
                Dictionary<long, List<RiotSharp.LeagueEndpoint.League>> dict = ApiFetcher.api.GetLeagues(Region.euw, summonerList);
                League league = dict.First().Value[0];

                Tier tier = league.Tier;

                String name = league.Tier.ToString();

                LeagueEntry entry = league.Entries[0];

                int wins = entry.Wins;
                int losses = entry.Losses;
                int points = entry.LeaguePoints;
                string division = entry.Division;
                return FileWriter.WriteToFile(name, division, points, wins, losses);

            } catch(Exception e)
            {
                Console.WriteLine(e);
                return FileWriter.WriteToFile("Placements");
            }
           
        }
    }
}
