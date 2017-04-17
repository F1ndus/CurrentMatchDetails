using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.CurrentGameEndpoint;
using RiotSharp.CurrentGameEndpoint.Enums;

namespace LoLCurrentMatchDetails
{
    class NormalCustomFetcher : GameFetcher
    {
        public string getQueueData(CurrentGame game, long summonerid)
        {
            GameQueueType type = game.GameQueueType;
            FileWriter.WriteToFile(type.ToString());
            return type.ToString();
        }
    }
}
