using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.CurrentGameEndpoint;
using RiotSharp;
using RiotSharp.TeamEndpoint;

namespace ProcessStartEventTest
{
    class TeamRankedFetcher : GameFetcher
    {
        public void getQueueData(CurrentGame game, long summonerid)
        {
            List<Participant> participants = game.Participants;
            long teamId = participants.Find(x => x.SummonerId.Equals(summonerid)).TeamId;
            Team teamInfo = null;
           // teamId == 100 ? teamInfo = game.Team

        }
    }
}
