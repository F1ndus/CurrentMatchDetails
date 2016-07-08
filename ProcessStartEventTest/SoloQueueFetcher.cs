using System;
using System.Collections.Generic;
using System.Linq;
using RiotSharp.CurrentGameEndpoint;
using RiotSharp;
using RiotSharp.LeagueEndpoint;

namespace ProcessStartEventTest
{
    class SoloQueueFetcher : GameFetcher
    {
        public void getQueueData(CurrentGame game,long summonerid)
        {
            Participant summoner = null;
            game.Participants.ForEach(user => {
                long userid = user.SummonerId;
                if(userid == summonerid)
                {
                    summoner = user;
                }
            });
            List<int> summonerList = new List<int>();
            summonerList.Add(unchecked((int)summoner.SummonerId));
            Dictionary<long,List<RiotSharp.LeagueEndpoint.League>> dict = ApiFetcher.api.GetLeagues(Region.euw,summonerList);
            League league = dict.First().Value[0];

            Tier tier = league.Tier;

            String name = league.Tier.ToString();
       
            LeagueEntry entry = league.Entries[0];
        
            int wins = entry.Wins;
            int losses = entry.Losses;
            int points = entry.LeaguePoints;
            string division = entry.Division;
            FileWriter.WriteToFile(name, division, points, wins, losses);
        }
    }
}
