using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.CurrentGameEndpoint;
using RiotSharp.CurrentGameEndpoint.Converters;

namespace ProcessStartEventTest
{
    class NormalCustomFetcher : GameFetcher
    {
        public void getQueueData(CurrentGame game, long summonerid)
        {
            GameQueueType type = game.GameQueueType;
            FileWriter.WriteToFile(type.ToString());
        }
    }
}
