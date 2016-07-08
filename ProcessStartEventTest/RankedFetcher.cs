using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.CurrentGameEndpoint;

namespace ProcessStartEventTest
{
    interface GameFetcher
    {
        void getQueueData(CurrentGame game,long summonerid);
    }
}